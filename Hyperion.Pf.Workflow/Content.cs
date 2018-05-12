using System;
using NLog;

namespace Hyperion.Pf.Workflow
{
    /// <summary>
    /// コンテンツ
    /// </summary>
    public abstract class Content
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 所属するパースペクティブ
        /// </summary>
        Perspective _Perspective;

        string _Name;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="contentName">コンテンツ名</param>
        public Content(string contentName)
        {
            this._Name = contentName;
            this.Status = ContentStatus.Create;
        }

        /// <summary>
        /// コンテントを終了可能状態にする
        /// </summary>
        protected void CompleteStop()
        {
            StopCompleteFlag = true;
            _Perspective.CompleteStop();
        }

        /// <summary>
        /// コンテントを開始可能状態にする
        /// </summary>
        protected void CompleteStart()
        {
            StartCompleteFlag = true;
            _Perspective.CompleteStart();
        }

        bool bProgressStop = false;

        bool bProgressSuspend = false;

        /// <summary>
        /// ロールバックでの戻り先
        /// </summary>
        ContentStatus TransitionRollbak = ContentStatus.Initialize;

        /// <summary>
        /// 現在の状態にあわせた終了ハンドラを呼び出す。
        /// ただし、Resumeは呼び出さない？
        /// PreXXXの関連を調査する
        /// </summary>
        /// <returns>状態の終了に成功した場合はTrueを返す</returns>
        internal void Forward()
        {
            _logger.Debug("Status={}", Status);
            if (Status == ContentStatus.Idle)
            {
                Status = ContentStatus.Resume;
                OnResume();
            }
            else if (Status == ContentStatus.Resume)
            {
                Status = ContentStatus.Run;
                OnRun();
            }
            else if (Status == ContentStatus.Run)
            {
                StartCompleteFlag = false;
                Status = ContentStatus.Restart;
                OnRestart();
                Status = ContentStatus.Run;
                OnRun();
            }
            else if (Status == ContentStatus.PreStop)
            {
                StopCompleteFlag = false;
                Status = ContentStatus.Stop;
                OnStop();
            }
            else if (Status == ContentStatus.Stop)
            {
                if (bProgressStop)
                {
                    Status = ContentStatus.End;
                    OnEnd();
                }
                else if (bProgressSuspend)
                {
                    Status = ContentStatus.Suspend;
                    OnSuspend();
                }
            }
            else if (Status == ContentStatus.End)
            {
                bProgressStop = false;
                bProgressSuspend = false;
                _Perspective = null;
                Status = ContentStatus.Idle;
                OnIdle();
            }
            else if (Status == ContentStatus.Suspend)
            {
                Status = ContentStatus.PreResume;
                OnPreResume();
            }
            else if (Status == ContentStatus.PreResume)
            {
                _logger.Debug("PreResumeのForward処理");
                Status = ContentStatus.Resume;
                OnResume();
            }
            else
            {
                throw new ApplicationException("状態が不正です");
            }
        }

        internal void Discontinue()
        {
            if (Status != ContentStatus.Suspend)
            {
                throw new ApplicationException("状態が不正です");
            }
            Status = ContentStatus.Discontinue;
            OnDiscontinue();
        }

        internal void Initialize()
        {
            if (Status != ContentStatus.Create)
            {
                throw new ApplicationException("状態が不正です");
            }

            Status = ContentStatus.Initialize;
            OnInitialize();
        }

        internal void Start()
        {
            Status = ContentStatus.Idle;
            OnIdle();
        }

        internal bool Stop()
        {
            _logger.Debug("コンテントを終了状態に遷移します(Content = {})", _Name);
            if (Status != ContentStatus.Run)
            {
                throw new ApplicationException("状態が不正です");
            }
            bProgressStop = true;
            bProgressSuspend = false;

            TransitionRollbak = ContentStatus.Run;
            Status = ContentStatus.PreStop;
            return OnPreStop(); // Stop状態へ遷移してよいか？
        }


        internal bool Suspend()
        {
            _logger.Debug("コンテントをサスペンド状態に遷移します(Content = {})", _Name);
            if (Status != ContentStatus.Run)
            {
                throw new ApplicationException("状態が不正です");
            }
            bProgressStop = false;
            bProgressSuspend = true;

            TransitionRollbak = ContentStatus.Run;
            Status = ContentStatus.PreStop;
            return OnPreStop(); // Stop状態へ遷移してよいか？
        }

        internal bool Begin(Perspective Perspective)
        {
            _logger.Debug("コンテントを開始状態に遷移します(Content = {})", _Name);
            if (Status != ContentStatus.Idle && Status != ContentStatus.Suspend)
            {
                throw new ApplicationException(string.Format("状態が不正です(Status={0})", Status));
            }
            _Perspective = Perspective;
            TransitionRollbak = ContentStatus.Idle;
            Status = ContentStatus.PreResume;
            return OnPreResume(); // Resume状態へ遷移してよいか？
        }

        internal void Dispose()
        {
            Status = ContentStatus.Destroy;
            OnDestroy();
        }


        /// <summary>
        /// コンテント名を取得します
        /// </summary>
        /// <returns></returns>
        public string Name { get { return _Name; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Perspective Perspective { get { return _Perspective; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentStatus Status { get; private set; }


        internal bool StopCompleteFlag { get; private set; }

        internal bool StartCompleteFlag { get; private set; }

        /// <summary>
        /// Initialize状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnInitialize();

        /// <summary>
        /// Idle状態遷移時のイベントハンドラ
        /// </summary>
        public abstract void OnIdle();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract void OnRestart();

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

        /// <summary>
        /// PreStop状態遷移時のイベントハンドラ
        /// </summary>
        /// <returns></returns>
        public abstract bool OnPreStop();

        /// <summary>
        /// PreResume状態遷移時のイベントハンドラ
        /// </summary>
        /// <returns></returns>
        public abstract bool OnPreResume();


    }
}