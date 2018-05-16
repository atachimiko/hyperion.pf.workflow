
using System;

namespace Hyperion.Pf.Workflow.StateMachine
{
    /// <summary>
    /// Pfに移動する
    /// </summary>
    public class WorkflowMessageEventArgs : EventArgs
    {
        private object _Param;

        public WorkflowMessageEventArgs(object param)
        {
            _Param = param;
        }
    }
}