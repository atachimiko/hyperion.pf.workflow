using System;
using System.Collections.Generic;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.AsyncMachine;
using Hyperion.Pf.Workflow.StateMachine.Eventsas;

namespace Hyperion.Pf.Workflow.StateMachine
{
    public class FrameStateMachine<T, W> : AsyncPassiveStateMachine<T, W>
        where T : IComparable
        where W : IComparable
    {
        public event EventHandler<InvokeShowFrameEventArgs> InvokeShowFrame;

        public event EventHandler<InvokeHideFrameEventArgs> InvokeHideFrame;

        /// <summary>
        /// 任意のフレームを表示にすることを通知する
        /// </summary>
        /// <param name="frameName"></param>
        /// <param name="menuList"></param>
        protected void ShowFrame(string frameName, ICollection<int> menuList)
        {
            Console.WriteLine("ShowFrame");
            //this.RaiseEvent(this.InvokeShowFrame, new InvokeShowFrameEventArgs(frameName, menuList), true);
        }

        /// <summary>
        /// 任意のフレームを非表示にすることを通知する
        /// </summary>
        /// <param name="frameName"></param>
        /// <param name="menuList"></param>
        protected void HideFrame(string frameName, ICollection<int> menuList)
        {
            //this.RaiseEvent(this.InvokeHideFrame, new InvokeHideFrameEventArgs(frameName, menuList), true);
        }

        private void RaiseEvent<EVPARAM>(EventHandler<EVPARAM> eventHandler, EVPARAM arguments, bool raiseEventOnException)
          where EVPARAM : EventArgs
        {
            try
            {
                if (eventHandler == null)
                {
                    return;
                }

                eventHandler(this, arguments);
            }
            catch (Exception)
            {
                if (!raiseEventOnException)
                {
                    throw;
                }
            }
        }
    }
}