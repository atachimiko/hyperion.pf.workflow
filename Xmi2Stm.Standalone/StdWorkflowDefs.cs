using System;
using Hyperion.Pf.Workflow;
using Hyperion.Pf.Workflow.StateMachine;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
namespace Pixstock.Core {
public class CLS_INIT : States {}
public class CLS_ROOT : States {}
public class CLS_CategoryTreeTop : States {}
public class CLS_CategoryTreeIdle : States {}
public class CLS_CategoryTreeLoading : States {}
public class CLS_TRNS_TOPSCREEN : Events {}
public class CLS_TRNS_EXIT : Events {}
public class CLS_TRNS_CategoryTreeLoading : Events {}
public class CLS_TRNS_CategoryTreeIdle : Events {}
public class CLS_TRNS_IDLE : Events {}
public class CLS_TRNS_BACK : Events {}
public class CLSINVALID_INVALID : Events {}
public partial class States : WorkflowStateBase {
	public static CLS_INIT INIT { get; } = new CLS_INIT();
	public static CLS_ROOT ROOT { get; } = new CLS_ROOT();
	public static CLS_CategoryTreeTop CategoryTreeTop { get; } = new CLS_CategoryTreeTop();
	public static CLS_CategoryTreeIdle CategoryTreeIdle { get; } = new CLS_CategoryTreeIdle();
	public static CLS_CategoryTreeLoading CategoryTreeLoading { get; } = new CLS_CategoryTreeLoading();
}
public partial class Events : WorkflowEventBase {
	public static CLS_TRNS_TOPSCREEN TRNS_TOPSCREEN { get; } = new CLS_TRNS_TOPSCREEN();
	public static CLS_TRNS_EXIT TRNS_EXIT { get; } = new CLS_TRNS_EXIT();
	public static CLS_TRNS_CategoryTreeLoading TRNS_CategoryTreeLoading { get; } = new CLS_TRNS_CategoryTreeLoading();
	public static CLS_TRNS_CategoryTreeIdle TRNS_CategoryTreeIdle { get; } = new CLS_TRNS_CategoryTreeIdle();
	public static CLS_TRNS_IDLE TRNS_IDLE { get; } = new CLS_TRNS_IDLE();
	public static CLS_TRNS_BACK TRNS_BACK { get; } = new CLS_TRNS_BACK();
	public static CLSINVALID_INVALID INVALID { get; } = new CLSINVALID_INVALID();
}
}
