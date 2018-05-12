using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hyperion.Pf.Workflow;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
using Pixstock;
using Pixstock.Core;
using Hyperion.Pf.Workflow.StateMachine;

namespace Pixstock.Applus.Foundations.Preview.Transitions
{
    /// <summary>
    /// 状態遷移図から、Xmi2Stmツールを使用して生成したワークフロークラス
    /// </summary>
    /// <typeparam name="States"></typeparam>
    /// <typeparam name="Events"></typeparam>
    public partial class ImagrePreviewTransitionWorkflow : FrameStateMachine<States, Events>, IAsyncPassiveStateMachine
    {
        public static string Name = "Pixstock.Preview.ImagrePreviewTransitionWorkflow";
        public void Setup()
        {
            DefineHierarchyOn(States.ROOT)
            .WithHistoryType(HistoryType.None)
            .WithInitialSubState(States.ImagePreviewTop)
            ;
            In(States.INIT)
            .On(Events.TRNS_TOPSCREEN)
            .Goto(States.ImagePreviewTop);
            In(States.ROOT)
            .On(Events.TRNS_EXIT)
            .Goto(States.INIT);
            In(States.ImagePreviewTop)
            .ExecuteOnEntry(__FTC_Event_ImagePreviewTop_Entry);
            In(States.ImagePreviewTop)
            .ExecuteOnExit(__FTC_Event_ImagePreviewTop_Exit);
            In(States.ImagePreviewTop)
            .On(Events.ACT_SHOW_NAVINEXT_PREVIEW)
            .Execute<object>(ACT_SHOW_NAVINEXT_PREVIEW);
            In(States.ImagePreviewTop)
            .On(Events.ACT_SHOW_NAVIPREV_PREVIEW)
            .Execute<object>(ACT_SHOW_NAVIPREV_PREVIEW);
            Initialize(States.INIT);
        }
        public virtual async Task __FTC_Event_ImagePreviewTop_Entry()
        {
            ICollection<int> ribbonMenuEventId = new List<int> { };
			// Nc版ではLivetを使わないので、ShowFrameを呼び出すだけ
            //await UIDispatcher.InvokeAsync(() => ShowFrame("Pixstock.Applus.Containers.ImagePreviewTop",ribbonMenuEventId));
            ShowFrame("Pixstock.Applus.Containers.ImagePreviewTop",ribbonMenuEventId);
        }
        public virtual async Task __FTC_Event_ImagePreviewTop_Exit()
        {
            ICollection<int> ribbonMenuEventId = new List<int> { };
			// Nc版ではLivetを使わないので、ShowFrameを呼び出すだけ
            //await UIDispatcher.InvokeAsync(() => HideFrame(ribbonMenuEventId));
        }
        public virtual async Task ACT_SHOW_NAVINEXT_PREVIEW(object param)
        {
            Events.ACT_SHOW_NAVINEXT_PREVIEW.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
            await OnACT_SHOW_NAVINEXT_PREVIEW(param);
            Events.ACT_SHOW_NAVINEXT_PREVIEW.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
        }
        public virtual async Task ACT_SHOW_NAVIPREV_PREVIEW(object param)
        {
            Events.ACT_SHOW_NAVIPREV_PREVIEW.FireInvokeWorkflowEvent(new WorkflowMessageEventArgs(param));
            await OnACT_SHOW_NAVIPREV_PREVIEW(param);
            Events.ACT_SHOW_NAVIPREV_PREVIEW.FireCallbackWorkflowEvent(new WorkflowMessageEventArgs(param));
        }
    }
}
