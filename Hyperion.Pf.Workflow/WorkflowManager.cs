using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyperion.Pf.Workflow
{
    public class WorkflowManager
    {
        /// <summary>
        /// 各フレームのコンテントスタック
        /// </summary>
        private readonly FrameList _FrameList = new FrameList();

        /// <summary>
        /// 定義済みパースペクティブ一覧
        /// </summary>
        /// <remarks>
        /// パースペクティブをスタック管理するためのリストです。
        /// スタック内のオブジェクトを使いまわすため、Stack型ではなくLinkedList型を使用しています。
        /// </remarks>
        private readonly LinkedList<Perspective> _DefinedPerspectiveList = new LinkedList<Perspective>();

        /// <summary>
        /// パースペクティブを開始する
        /// </summary>
        /// <param name="PerspectiveName"></param>
        public void StartPerspective(string PerspectiveName)
        {
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
                    // TODO:
                    break;
                case ArbitrationMode.OVERRIDE:
                    // TODO:
                    break;
                case ArbitrationMode.INTRCOMP:
                    // TODO:
                    break;
                case ArbitrationMode.INTRCOOR:
                    // TODO:
                    break;
                default:
                    throw new ApplicationException("未サポート");
            }

            // 2. 作用したコンテントのライフサイクル処理
            //    ・終了コンテントのライフサイクル
            //    ・開始コンテントのライフサイクル
            foreach (var endContent in result.EndContentList)
            {
                endContent.End();
            }

            foreach (var startContent in result.StartContentList)
            {
                startContent.Start();
            }

            // 3. パースペクティブのライフサイクル処理
            // TODO;
            foreach (var endContent in result.EndContentList)
            {
                DoPerspectiveEnd(endContent.Perspective);
            }

            // パースペクティブから呼び出す
            //foreach(var startContent in result.StartContentList) {
            //    startContent.Run();
            //}
        }

        /// <summary>
        /// パースペクティブの終了判定
        /// </summary>
        /// <param name="perspective"></param>
        private void DoPerspectiveEnd(Perspective perspective)
        {
            // 終了判定で、パースペクティブが終了するかチェックする
            //   すべてのコンテントが終了状態であるならば、パースペクティブの終了処理を実施する
            bool bEnded = true;
            foreach (Content attachContent in perspective.Contents)
            {
                if (attachContent.Status != ContentStatus.Destroy)
                {
                    bEnded = false;
                    break;
                }
            }

            if (bEnded)
            {
                // パースペクティブの終了処理
                //   すべてのコンテントの破棄ライフサイクルを実行する
                foreach (Content attachContent in perspective.Contents)
                {
                    attachContent.Destroy();
                }

                foreach (Content attachContent in perspective.Contents)
                {
                    attachContent.Dispose();
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
                attachContent.Run();
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
            DoArbitrationResult result = new DoArbitrationResult();

            foreach (var frameName in pPerspective.FrameList)
            {
                bool bPreEndSuccess = true;
                bool bPreStartSuccess = false;
                Content wEndContent = null;
                Content startContent = null;

                // コンテントを作成
                var builder = pPerspective.GetContentBuilder(frameName);
                var stack = _FrameList.GetContentStack(frameName);
                if (stack.Count > 0)
                {
                    wEndContent = stack.Peek();
                    bPreEndSuccess = wEndContent.PreEnd();
                }

                if (bPreEndSuccess)
                {
                    // コンテント実態を作成
                    startContent = builder.Build(pPerspective);
                    startContent.OnInitialize();

                    bPreStartSuccess = startContent.PreStart();

                    if (bPreStartSuccess)
                    {
                        wEndContent = stack.Pop(); // スタックから除去する

                        // 新しいパースペクティブのコンテントをスタックに積む
                        stack.Push(startContent);

                        pPerspective.AddContent(frameName, startContent);
                        result.EndContentList.Add(wEndContent);
                        result.StartContentList.Add(startContent);
                    }
                    else
                    {
                        // TODO: Stackのコンテント(wEndContent)のロールバック
                    }
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

        /// <summary>
        /// フレームのコンテントスタックを取得する
        /// </summary>
        /// <param name="FrameName">フレーム名</param>
        /// <returns></returns>
        public FrameContentStack GetContentStack(string FrameName)
        {
            return mFrameDict[FrameName];
        }
    }

    public class FrameContentStack : Stack<Content>
    {

    }
}
