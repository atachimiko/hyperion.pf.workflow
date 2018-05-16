using System;

namespace Hyperion.Pf.Workflow.StateMachine
{
    public delegate int _FireInvokeWorkflowEvent(WorkflowMessageEventArgs param);
    public delegate int _FireCallbackWorkflowEvent(WorkflowMessageEventArgs param);

    public class WorkflowEventBase : IComparable
    {
        public event _FireInvokeWorkflowEvent InvokeWorkflowEvent;
        public event _FireCallbackWorkflowEvent CallbackWorkflowEvent;

        public void FireInvokeWorkflowEvent(WorkflowMessageEventArgs args)
        {
            if (InvokeWorkflowEvent != null)
                InvokeWorkflowEvent.Invoke(args);
        }

        public void FireCallbackWorkflowEvent(WorkflowMessageEventArgs args)
        {
            if (CallbackWorkflowEvent != null)
                CallbackWorkflowEvent.Invoke(args);
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class WorkflowStateBase : IComparable
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}