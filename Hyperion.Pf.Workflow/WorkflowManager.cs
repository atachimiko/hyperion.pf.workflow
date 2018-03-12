using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Hyperion.Pf.Workflow
{
    class WorkflowManagerContext
    {
        List<Content> waitCompleteStopContent = new List<Content>();

        List<Content> waitCompleteStartContent = new List<Content>();

        public void addWaitCompleteStopContent(Content content)
        {
            waitCompleteStopContent.Add(content);
        }

        public void addWaitCompleteStartContent(Content content)
        {
            waitCompleteStartContent.Add(content);
        }

    }

    /// <summary>
    /// ワークフロー管理クラス
    /// </summary>
    public class WorkflowManager
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private bool _VerifiedFlag = false;

        private bool _PerspectiveProcessingStartFlag = false; // パースペクティブ起動中処理フラグ

        /// <summary>
        /// 各フレームのコンテントスタック
        /// </summary>
        internal readonly FrameList _FrameList = new FrameList();

        internal void CompleteStop()
        {
            _logger.Debug("IN");
            // すべてのStop状態のコンテントが、CompleteStopを呼び出しているかチェックする
            var r = from u in Contents
                    where (u.Status == ContentStatus.PreStop || u.Status == ContentStatus.Stop) && u.StopCompleteFlag == false
                    select u;

            // すべてのStop状態コンテントがCompleteStop済みならば、
            if (r.Count() == 0)
            {
                // PreResume状態のコンテントをResume状態へ遷移する

                var r_preresumecontent = from u in Contents
                                         where u.Status == ContentStatus.PreResume
                                         select u;

                WorkflowManagerContext context = new WorkflowManagerContext();
                foreach (var content in r_preresumecontent)
                {
                    content.Forward(); // PreResume状態コンテントのForwardなので、Resume状態に遷移する
                }
            }
            _logger.Debug("OUT");
        }

        internal void CompleteStart()
        {
            _logger.Debug("IN");

            // すべてのResume状態のコンテントが、CompleteStartを呼び出しているかチェックする
            var r = from u in Contents
                    where u.Status == ContentStatus.Resume && u.StartCompleteFlag == false
                    select u;

            // すべてのResume状態コンテントがCompleteStart済みならば、
            if (r.Count() == 0)
            {
                // Stop状態のコンテントを、Suspend/End状態へ遷移する
                var r_prestopcontent = from u in Contents
                                       where u.Status == ContentStatus.Stop
                                       select u;

                WorkflowManagerContext context = new WorkflowManagerContext();
                foreach (var content in r_prestopcontent)
                {
                    content.Forward(); // Stop状態コンテントのForwardなので、Suspend/End状態に遷移する
                }

                // Resume状態コンテントのForward(Run状態へ遷移)
                var rContentResume = from u in Contents
                                     where u.Status == ContentStatus.Resume
                                     select u;
                foreach (var content in rContentResume)
                {
                    DoPerspectiveStart(content.Perspective);
                }

                // End状態コンテントのForward(Idle状態へ遷移)
                var rContentEnd = from u in Contents
                                  where u.Status == ContentStatus.End
                                  select u;
                foreach (var content in rContentEnd)
                {
                    DoPerspectiveEnd(content.Perspective);
                }

                _PerspectiveProcessingStartFlag = false; // パースペクティブの起動完了
            }
            _logger.Debug("OUT");
        }

        /// <summary>
        /// 定義済みパースペクティブ一覧
        /// </summary>
        /// <remarks>
        /// パースペクティブをスタック管理するためのリストです。
        /// スタック内のオブジェクトを使いまわすため、Stack型ではなくLinkedList型を使用しています。
        /// </remarks>
        private readonly LinkedList<Perspective> _DefinedPerspectiveList = new LinkedList<Perspective>();

        /// <summary>
        /// 定義済みコンテント一覧
        /// </summary>
        /// <remarks>
        /// ワークフローが管理するコンテント一覧です。
        /// パースペクティブは、この一覧の中に含まれているコンテントのみを使用できます。
        /// </remarks>
        /// <returns></returns>
        private readonly Dictionary<string, Content> _DeclaredContentList = new Dictionary<string, Content>();

        /// <summary>
        /// 定義済みコンテント一覧を取得する
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Content> Contents
        {
            get
            {
                return _DeclaredContentList.Values;
            }
        }

        /// <summary>
        /// コンテントの初期化
        /// </summary>
        /// <param name="builderList"></param>
        public void Verify(List<IContentBuilder> builderList)
        {
            foreach (var builder in builderList)
            {
                var content = builder.Build();
                _DeclaredContentList.Add(content.Name, content);
            }

            foreach (var content in Contents)
            {
                content.Initialize();
                content.Start();
            }

            _VerifiedFlag = true;
        }

        internal Content GetContent(string contentName)
        {
            return _DeclaredContentList[contentName];
        }

        /// <summary>
        /// パースペクティブを開始する
        /// </summary>
        /// <param name="PerspectiveName">開始するパースペクティブ名</param>
        public void StartPerspective(string PerspectiveName)
        {
            if (!_VerifiedFlag)
            {
                throw new ApplicationException("初期化が完了していません");
            }
            if (_PerspectiveProcessingStartFlag)
            {
                throw new ApplicationException("パースペクティブ起動中です");
            }
            _PerspectiveProcessingStartFlag = true;

            var target = FindPerspective(PerspectiveName);
            if (target == null)
            {
                throw new ApplicationException("存在しないパースペクティブ名です");
            }

            // 1. パースペクティブの調停
            DoArbitrationResult result = null;
            switch (target.Mode)
            {
                case ArbitrationMode.AWAB:
                    result = doArbitration_AWAB(target);
                    break;
                case ArbitrationMode.BWAB:
                    result = doArbitration_BWAB(target);
                    break;
                case ArbitrationMode.OVERRIDE:
                    result = doArbitration_OVERRIDE(target);
                    break;
                case ArbitrationMode.INTRCOMP:
                    result = doArbitration_INTRCOMP(target);
                    break;
                case ArbitrationMode.INTRCOOR:
                    result = doArbitration_INTRCOOR(target);
                    break;
                default:
                    throw new ApplicationException("未サポート");
            }

            if (result.EndContentList.Count == 0)
            {
                _logger.Debug("終了するコンテントがないため、新しいコンテントを起動します");
                foreach (var content in result.StartContentList)
                {
                    content.Forward();
                }
            }
            else
            {
                foreach (var content in result.EndContentList)
                {
                    content.Forward();
                }
            }
        }

        /// <summary>
        /// パースペクティブを登録する
        /// </summary>
        /// <param name="perspective">登録するパースペクティブ</param>
        public void RegisterPerspective(Perspective perspective)
        {
            if (FindPerspective(perspective.Name) != null)
            {
                throw new ArgumentException("同一名称のパースペクティブが定義済みです");
            }

            if (perspective.Status != PerspectiveStatus.Deactive)
            {
                throw new ArgumentException("登録できないパースペクティブです");
            }

            _DefinedPerspectiveList.AddLast(perspective);
        }

        /// <summary>
        /// パースペクティブの終了判定
        /// </summary>
        /// <param name="perspective"></param>
        private void DoPerspectiveEnd(Perspective perspective)
        {
            // 終了判定で、パースペクティブが終了するかチェックする
            //   すべてのコンテントが終了状態であるならば、パースペクティブの終了処理を実施する
            bool perspectiveEndFlag = true;
            foreach (Content attachContent in perspective.Contents)
            {
                if (attachContent.Status != ContentStatus.Destroy)
                {
                    perspectiveEndFlag = false;
                    break;
                }
            }

            if (perspectiveEndFlag)
            {
                // パースペクティブの終了処理
                //   すべてのコンテントの破棄ライフサイクルを実行する
                foreach (Content attachContent in perspective.Contents)
                {
                    attachContent.Stop();
                }

                perspective.Status = PerspectiveStatus.Deactive;
                _DefinedPerspectiveList.Remove(perspective);
                _DefinedPerspectiveList.AddLast(perspective);
            }
        }

        /// <summary>
        /// パースペクティブの開始判定
        /// </summary>
        /// <param name="perspective"></param>
        private void DoPerspectiveStart(Perspective perspective)
        {
            foreach (Content attachContent in perspective.Contents)
            {
                attachContent.Forward();
            }

            perspective.Status = PerspectiveStatus.Active;

            _DefinedPerspectiveList.Remove(perspective);
            _DefinedPerspectiveList.AddFirst(perspective);
        }

        /// <summary>
        /// 調停モード処理の結果
        /// </summary>
        class DoArbitrationResult
        {
            private readonly List<Content> mEndContentList = new List<Content>();

            private readonly List<Content> mStartContentList = new List<Content>();

            /// <summary>
            /// 調停処理により終了するコンテント一覧を取得する
            /// </summary>
            /// <returns></returns>
            public List<Content> EndContentList { get { return mEndContentList; } }

            /// <summary>
            /// 調停処理により開始するコンテント一覧を取得する
            /// </summary>
            /// <returns></returns>
            public List<Content> StartContentList { get { return mStartContentList; } }
        }

        /// <summary>
        /// 後勝調停モード
        /// </summary>
        private DoArbitrationResult doArbitration_AWAB(Perspective pPerspective)
        {
            _logger.Debug("AWAB調停処理を開始します");
            DoArbitrationResult result = new DoArbitrationResult();
            WorkflowManagerContext context = new WorkflowManagerContext();

            foreach (var frameName in pPerspective.FrameList)
            {
                bool bPreEndSuccess = true;
                bool bPreStartSuccess = false;
                Content wEndContent = null;
                Content startContent = pPerspective.GetFrameContent(frameName);

                var stack = _FrameList.GetContentStack(frameName);
                if (stack.Count > 0)
                {
                    _logger.Debug("{}フレームの最上位コンテントの終了ライフサイクル処理を開始します", frameName);
                    wEndContent = stack.Peek();
                    if (wEndContent != startContent)
                    {
                        bPreEndSuccess = wEndContent.Stop();
                    }
                }

                if (bPreEndSuccess)
                {
                    startContent = pPerspective.GetFrameContent(frameName);

                    _logger.Debug("{}フレームに新規コンテントの開始ライフサイクル処理を開始します", frameName);
                    bPreStartSuccess = startContent.Begin(pPerspective);

                    if (bPreStartSuccess)
                    {
                        if (stack.Count > 0)
                        {
                            wEndContent = stack.Pop(); // スタックから除去する
                            _logger.Debug("{}フレームのコンテントスタックから除去します", frameName);
                            result.EndContentList.Add(wEndContent);
                        }

                        _logger.Debug("{}フレームのコンテントスタックに新規コンテントを追加します。", frameName);
                        // 新しいパースペクティブのコンテントをスタックに積む
                        stack.Push(startContent);

                        result.StartContentList.Add(startContent);
                    }
                    else
                    {
                        _logger.Warn("{}フレームの新規コンテントを追加に失敗しました。ロールバックを行います。", frameName);
                        // TODO: Stackのコンテント(wEndContent)のロールバック
                    }
                }
                else
                {
                    _logger.Warn("{}フレームの最上位コンテントの終了ライフサイクル処理に失敗しました", frameName);
                }
            }

            return result;
        }

        private DoArbitrationResult doArbitration_BWAB(Perspective pPerspective)
        {
            _logger.Debug("BWAB調停処理を開始します");
            DoArbitrationResult result = new DoArbitrationResult();

            foreach (var frameName in pPerspective.FrameList)
            {
                bool bPreStartSuccess = false;
                Content startContent = null;

                // コンテントを作成
                var stack = _FrameList.GetContentStack(frameName);
                if (stack.Count > 0)
                {
                    _logger.Debug("{}フレームの最上位コンテントが存在するためスキップします。", frameName);
                    continue;
                }

                startContent = pPerspective.GetFrameContent(frameName);

                _logger.Debug("{}フレームに新規コンテントの開始ライフサイクル処理を開始します", frameName);
                bPreStartSuccess = startContent.Begin(pPerspective);

                if (bPreStartSuccess)
                {
                    _logger.Debug("{}フレームのコンテントスタックに新規コンテントを追加します。", frameName);
                    // 新しいパースペクティブのコンテントをスタックに積む
                    stack.Push(startContent);

                    result.StartContentList.Add(startContent);
                }
                else
                {
                    _logger.Warn("{}フレームの新規コンテントを追加に失敗しました。ロールバックを行います。", frameName);
                    // TODO: Stackのコンテント(wEndContent)のロールバック
                }
            }

            return result;
        }

        private DoArbitrationResult doArbitration_OVERRIDE(Perspective pPerspective)
        {
            _logger.Debug("OVERRIDE調停処理を開始します");
            DoArbitrationResult result = new DoArbitrationResult();

            List<Perspective> clearPerspectiveList = new List<Perspective>(); // クリアするPerspective

            List<Content> restartContentList = new List<Content>();

            // すべてのフレームを巡回し、コンテントを終了する(終了できないコンテントがある場合は、ロールバックする)

            foreach (var frameName in _FrameList.FrameNameList)
            {
                var stack = _FrameList.GetContentStack(frameName);
                foreach (var content in stack)
                {
                    if (pPerspective.Contents.Contains(content))
                    {
                        // 起動しようとしているPerspectiveと同じコンテントが含まれている場合は、
                        // 終了判定は行わない。
                        restartContentList.Add(content);
                    }
                    else
                    {
                        bool bEnd = content.Stop();
                        if (bEnd)
                        {
                            result.EndContentList.Add(content);
                        }
                        else
                        {
                            // result.EndContentListのすべてのContentをロールバックする
                        }
                    }
                }

                stack.Clear(); // コンテントスタックから、すべてのコンテントを除去し、スタックを空にする。
            }

            foreach (var frameName in pPerspective.FrameList)
            {
                bool bPreStartSuccess = false;
                Content startContent = null;

                var stack = _FrameList.GetContentStack(frameName);
                startContent = pPerspective.GetFrameContent(frameName);

                _logger.Debug("{}フレームに新規コンテントの開始ライフサイクル処理を開始します", frameName);
                bPreStartSuccess = startContent.Begin(pPerspective);

                if (bPreStartSuccess)
                {
                    _logger.Debug("{}フレームのコンテントスタックに新規コンテントを追加します。", frameName);
                    // 新しいパースペクティブのコンテントをスタックに積む
                    stack.Push(startContent);

                    result.StartContentList.Add(startContent);
                }
                else
                {
                    _logger.Warn("{}フレームの新規コンテントを追加に失敗しました。ロールバックを行います。", frameName);
                    // TODO: Stackのコンテント(wEndContent)のロールバック
                }
            }

            return result;
        }

        private DoArbitrationResult doArbitration_INTRCOMP(Perspective pPerspective) {
            _logger.Debug("INTRCOMP調停処理を開始します");
            DoArbitrationResult result = new DoArbitrationResult();

            List<Perspective> clearPerspectiveList = new List<Perspective>(); // クリアするPerspective

            List<Content> restartContentList = new List<Content>();

            // すべてのフレームを巡回し、コンテントをサスペンド状態にする

            foreach (var frameName in _FrameList.FrameNameList)
            {
                var stack = _FrameList.GetContentStack(frameName);
                foreach (var content in stack)
                {
                    if (pPerspective.Contents.Contains(content))
                    {
                        // 起動しようとしているPerspectiveと同じコンテントが含まれている場合は、
                        // 終了判定は行わない。
                        restartContentList.Add(content);
                    }
                    else
                    {
                        bool bEnd = content.Suspend();
                        if (bEnd)
                        {
                            result.EndContentList.Add(content);
                        }
                        else
                        {
                            // result.EndContentListのすべてのContentをロールバックする
                        }
                    }
                }
            }

            foreach (var frameName in pPerspective.FrameList)
            {
                bool bPreStartSuccess = false;
                Content startContent = null;

                var stack = _FrameList.GetContentStack(frameName);
                startContent = pPerspective.GetFrameContent(frameName);

                _logger.Debug("{}フレームに新規コンテントの開始ライフサイクル処理を開始します", frameName);
                bPreStartSuccess = startContent.Begin(pPerspective);

                if (bPreStartSuccess)
                {
                    _logger.Debug("{}フレームのコンテントスタックに新規コンテントを追加します。", frameName);
                    // 新しいパースペクティブのコンテントをスタックに積む
                    stack.Push(startContent);

                    result.StartContentList.Add(startContent);
                }
                else
                {
                    _logger.Warn("{}フレームの新規コンテントを追加に失敗しました。ロールバックを行います。", frameName);
                    // TODO: Stackのコンテント(wEndContent)のロールバック
                }
            }

            return result;
        }

        private DoArbitrationResult doArbitration_INTRCOOR(Perspective pPerspective) {
            _logger.Debug("INTRCOOR調停処理を開始します");
            DoArbitrationResult result = new DoArbitrationResult();

            List<Perspective> clearPerspectiveList = new List<Perspective>(); // クリアするPerspective

            List<Content> restartContentList = new List<Content>();

            // すべてのフレームを巡回し、コンテントをサスペンド状態にする

            foreach (var frameName in pPerspective.FrameList)
            {
                var stack = _FrameList.GetContentStack(frameName);
                foreach (var content in stack)
                {
                    if (pPerspective.Contents.Contains(content))
                    {
                        // 起動しようとしているPerspectiveと同じコンテントが含まれている場合は、
                        // 終了判定は行わない。
                        restartContentList.Add(content);
                    }
                    else
                    {
                        bool bEnd = content.Suspend();
                        if (bEnd)
                        {
                            result.EndContentList.Add(content);
                        }
                        else
                        {
                            // result.EndContentListのすべてのContentをロールバックする
                        }
                    }
                }
            }

            foreach (var frameName in pPerspective.FrameList)
            {
                bool bPreStartSuccess = false;
                Content startContent = null;

                var stack = _FrameList.GetContentStack(frameName);
                startContent = pPerspective.GetFrameContent(frameName);

                _logger.Debug("{}フレームに新規コンテントの開始ライフサイクル処理を開始します", frameName);
                bPreStartSuccess = startContent.Begin(pPerspective);

                if (bPreStartSuccess)
                {
                    _logger.Debug("{}フレームのコンテントスタックに新規コンテントを追加します。", frameName);
                    // 新しいパースペクティブのコンテントをスタックに積む
                    stack.Push(startContent);

                    result.StartContentList.Add(startContent);
                }
                else
                {
                    _logger.Warn("{}フレームの新規コンテントを追加に失敗しました。ロールバックを行います。", frameName);
                    // TODO: Stackのコンテント(wEndContent)のロールバック
                }
            }

            return result;
        }

        private Perspective FindPerspective(string PerspectiveName)
        {
            return (from u in _DefinedPerspectiveList
                    where u.Name == PerspectiveName
                    select u).FirstOrDefault();
        }
    }

    /// <summary>
    /// フレームの管理と各フレームのコンテントスタックを保持するクラス
    /// </summary>
    public class FrameList
    {
        /// <summary>
        /// 各フレームのコンテントスタック
        /// </summary>
        /// <remarks>
        /// Key=フレーム名
        /// Value=コンテントスタック
        /// </remarks>
        /// <returns></returns>
        private readonly Dictionary<string, FrameContentStack> mFrameDict = new Dictionary<string, FrameContentStack>();

        internal string[] FrameNameList { get { return mFrameDict.Keys.ToArray(); } }

        /// <summary>
        /// フレームのコンテントスタックを取得する
        /// </summary>
        /// <param name="FrameName">フレーム名</param>
        /// <returns></returns>
        public FrameContentStack GetContentStack(string FrameName)
        {
            if (!mFrameDict.ContainsKey(FrameName))
            {
                mFrameDict.Add(FrameName, new FrameContentStack());
            }

            return mFrameDict[FrameName];
        }
    }

    public class FrameContentStack : Stack<Content>
    {

    }
}
