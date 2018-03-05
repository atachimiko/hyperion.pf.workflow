using System;

namespace Hyperion.Pf.Workflow
{
    public enum ContentStatus
    {
        Create,

        Initialize,
        Start,
        Run,
        Restart,
        Suspend,
        Resume,
        PreResume,
        Discontinue,
        PreDestroy,
        Destroy,
        End
    }

    /// <summary>
    /// ワークフローのインターフェース
    /// </summary>
    public abstract class Content : IDisposable
    {
        /// <summary>
        /// 所属するパースペクティブ
        /// </summary>
        Perspective _Perspective;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="attach">所属するパースペクティブ</param>
        public Content(Perspective attach)
        {
            this._Perspective = attach;

            this.Status = ContentStatus.Create;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool PreEnd()
        {
            bool bCheck = false;

            if (Status == ContentStatus.Run)
            {
                bCheck = OnPreDestroy();
                if (bCheck)
                    Status = ContentStatus.PreDestroy;
            }
            if (Status == ContentStatus.Suspend)
            {
                bCheck = OnPreResume();
                if (bCheck)
                    Status = ContentStatus.PreResume;
            }
            return bCheck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool PreStart()
        {
            bool bCheck = false;
            bCheck = OnStart();
            if (bCheck)
                Status = ContentStatus.Start;
            return bCheck;
        }

        /// <summary>
        /// 終了ライフサイクル
        /// </summary>
        /// <returns></returns>
        internal void End()
        {
            if (Status == ContentStatus.PreResume)
            {
                OnDiscontinue();
                Status = ContentStatus.Discontinue;
            }

            OnDestroy();
            Status = ContentStatus.Destroy;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Run()
        {
            if (Status == ContentStatus.Run)
            {
                Status = ContentStatus.Restart;
                OnRestart();
            }

            Status = ContentStatus.Run;

            // OnRunは、実際にはワークフローメッセージを送信する
            // このワークフローメッセージは、初期化メッセージとする。

            // TODO: OnRun
        }

        /// <summary>
        /// 開始ライフサイクル
        /// </summary>
        /// <returns></returns>
        internal void Start()
        {
            OnResume();
            Status = ContentStatus.Resume;
        }

        /// <summary>
        /// 破棄ライフサイクル
        /// </summary>
        public void Destroy()
        {
            OnEnd();
            Status = ContentStatus.End;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            _Perspective = null;
        }

        public Perspective Perspective { get { return _Perspective; } }

        public ContentStatus Status { get; private set; }

        /// <summary>
        /// Initialize状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnInitialize();

        /// <summary>
        /// Start状態遷移時のイベントハンドラ
        /// </summary>
        public abstract bool OnStart();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool OnRestart();

        /// <summary>
        /// Resume状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnResume();

        /// <summary>
        /// Run状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnRun();

        /// <summary>
        /// Suspend状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnSuspend();

        /// <summary>
        /// PreDestroy状態遷移時のイベントハンドラ
        /// </summary>
        /// <returns></returns>
        public abstract bool OnPreDestroy();

        /// <summary>
        /// PreResume状態遷移時のイベントハンドラ
        /// </summary>
        /// <returns></returns>
        public abstract bool OnPreResume();

        /// <summary>
        /// Discontinue状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnDiscontinue();

        /// <summary>
        /// Stop状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnStop();

        /// <summary>
        /// Destroy状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnDestroy();

        /// <summary>
        /// End状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnEnd();
    }
}