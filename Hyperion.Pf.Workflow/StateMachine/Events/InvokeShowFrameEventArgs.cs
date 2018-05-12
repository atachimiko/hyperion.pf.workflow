using System;
using System.Collections.Generic;

namespace Hyperion.Pf.Workflow.StateMachine.Eventsas
{
    public class InvokeShowFrameEventArgs : EventArgs
    {
        readonly string mFrameName;

        readonly ICollection<int> mMenuList;

        public string FrameName => this.mFrameName;

        public ICollection<int> MenuList => this.mMenuList;

        public InvokeShowFrameEventArgs(string frameName, ICollection<int> menuList)
        {
            this.mFrameName = frameName;
            this.mMenuList = menuList;
        }

    }
}