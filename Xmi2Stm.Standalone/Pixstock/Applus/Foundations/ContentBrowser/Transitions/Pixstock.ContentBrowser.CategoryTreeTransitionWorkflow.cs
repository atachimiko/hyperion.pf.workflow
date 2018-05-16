using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hyperion.Pf.Workflow;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
using Hyperion.Pf.Workflow.StateMachine;

using Pixstock.Core;
namespace Pixstock.Applus.Foundations.ContentBrowser.Transitions {
public partial class CategoryTreeTransitionWorkflow : FrameStateMachine<States, Events>, IAsyncPassiveStateMachine {
	public static string Name = "Pixstock.ContentBrowser.CategoryTreeTransitionWorkflow";
public void Setup() {
DefineHierarchyOn(States.ROOT)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.CategoryTreeTop)
;
DefineHierarchyOn(States.CategoryTreeTop)
.WithHistoryType(HistoryType.None)
.WithInitialSubState(States.CategoryTreeIdle)
.WithSubState(States.CategoryTreeLoading)
;
In(States.INIT)
.On(Events.TRNS_TOPSCREEN)
.Goto(States.CategoryTreeTop);
In(States.ROOT)
.On(Events.TRNS_EXIT)
.Goto(States.INIT);
In(States.CategoryTreeTop)
.On(Events.TRNS_IDLE)
.Goto(States.CategoryTreeIdle);
In(States.CategoryTreeIdle)
.On(Events.TRNS_CategoryTreeLoading)
.Goto(States.CategoryTreeLoading);
In(States.CategoryTreeIdle)
.On(Events.TRNS_BACK)
.Goto(States.CategoryTreeTop);
In(States.CategoryTreeLoading)
.On(Events.TRNS_CategoryTreeIdle)
.Goto(States.CategoryTreeIdle);
In(States.CategoryTreeTop)
.ExecuteOnEntry(CategoryTreeTop_Entry);
In(States.CategoryTreeTop)
.ExecuteOnExit(CategoryTreeTop_Exit);
In(States.CategoryTreeIdle)
.ExecuteOnEntry(CategoryTreeIdle_Entry);
In(States.CategoryTreeIdle)
.ExecuteOnExit(CategoryTreeIdle_Exit);
In(States.CategoryTreeLoading)
.ExecuteOnEntry(CategoryTreeLoading_Entry);
In(States.CategoryTreeLoading)
.ExecuteOnExit(CategoryTreeLoading_Exit);
	Initialize(States.INIT);
}
public virtual async Task CategoryTreeTop_Entry() {
	await OnCategoryTreeTop_Entry();
ICollection<int> ribbonMenuEventId = new List<int>{  };
	ShowFrame("CategoryTreeTop",ribbonMenuEventId);
}
public virtual async Task CategoryTreeTop_Exit() {
	await OnCategoryTreeTop_Exit();
ICollection<int> ribbonMenuEventId = new List<int>{  };
	HideFrame("CategoryTreeTop", ribbonMenuEventId);
}
public virtual async Task CategoryTreeIdle_Entry() {
	await OnCategoryTreeIdle_Entry();
}
public virtual async Task CategoryTreeIdle_Exit() {
	await OnCategoryTreeIdle_Exit();
}
public virtual async Task CategoryTreeLoading_Entry() {
	await OnCategoryTreeLoading_Entry();
}
public virtual async Task CategoryTreeLoading_Exit() {
	await OnCategoryTreeLoading_Exit();
}
}
}
