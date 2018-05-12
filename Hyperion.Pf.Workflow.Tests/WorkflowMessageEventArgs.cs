
using System;

namespace Pixstock.Core
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