using System;
using Hyperion.Pf.Workflow;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Infrastructure;
using Appccelerate.StateMachine.Machine;
using Appccelerate.StateMachine.Persistence;
using Appccelerate.StateMachine.Reports;
using Hyperion.Pf.Workflow.StateMachine;

namespace Pixstock.Core
{



    public class CLS_INIT : States { }
    public class CLS_ROOT : States { }
    public class CLS_CategoryTreeTop : States { }
    public class CLS_CategoryTreeIdle : States { }
    public class CLS_CategoryTreeLoading : States { }
    public class CLS_CategoryEditDialog : States { }
    public class CLS_RUN : States { }
    public class CLS_ContentBrowserTop : States { }
    public class CLS_ContentBrowserIdle : States { }
    public class CLS_ContentBrowserLoading : States { }
    public class CLS_ImagePreviewTop : States { }
    public class CLS_IDLE : States { }
    public class CLS_LOADING : States { }
    public class CLS_LOADING_PREVIEW : States { }
    public class CLS_LOADING_CONTENTLIST : States { }
    public class CLS_ACT_SELECTED_ITEM : Events { }
    public class CLS_ACT_SAVE_CATEGORY_ONCATEGORYEDIT : Events { }
    public class CLS_TRNS_TOPSCREEN : Events { }
    public class CLS_TRNS_EXIT : Events { }
    public class CLS_TRNS_CategoryTreeLoading : Events { }
    public class CLS_TRNS_CategoryTreeIdle : Events { }
    public class CLS_TRNS_DISP_CATEGORYEDIT : Events { }
    public class CLS_TRNS_IDLE : Events { }
    public class CLS_ACT_SHOW_NAVIPREVIEW : Events { }
    public class CLS_ACT_SHOW_SELECTEDPREVIEW : Events { }
    public class CLS_TRNS_LOADING : Events { }
    public class CLS_ACT_SHOW_NAVINEXT_PREVIEW : Events { }
    public class CLS_ACT_SHOW_NAVIPREV_PREVIEW : Events { }
    public class CLS_ACT_CONNECTING_SERVICE : Events { }
    public class CLS_ACT_GETARTIFACTLIST : Events { }
    public class CLS_ACT_LOAD_PREVIEW : Events { }
    public class CLS_ACT_LOAD_CATEGORY : Events { }
    public class CLS_ACT_PUT_CATEGORY : Events { }
    public class CLS_ACT_POST_CATEGORY : Events { }
    public class CLS_ACT_GET_CATEGORY : Events { }
    public class CLS_ACT_FETCH_CATEGORY : Events { }
    public class CLS_TRNS_CONNECTED : Events { }
    public class CLS_TRNS_DISCONNECTED : Events { }
    public class CLS_ACT_SAVE_CATEGORY : Events { }
    public class CLS_TRNS_RUN : Events { }
    public class CLS_TRNS_LOADING_PREVIEW : Events { }
    public class CLS_TRNS_INIT : Events { }
    public class CLS_TRNS_LOADING_CONTENTLIST : Events { }
    public class CLS_ACT_LOAD_CATEGORYLIST : Events { }
    public class CLSINVALID_INVALID : Events { }
    public partial class States : WorkflowStateBase
    {
        public static CLS_INIT INIT { get; } = new CLS_INIT();
        public static CLS_ROOT ROOT { get; } = new CLS_ROOT();
        public static CLS_CategoryTreeTop CategoryTreeTop { get; } = new CLS_CategoryTreeTop();
        public static CLS_CategoryTreeIdle CategoryTreeIdle { get; } = new CLS_CategoryTreeIdle();
        public static CLS_CategoryTreeLoading CategoryTreeLoading { get; } = new CLS_CategoryTreeLoading();
        public static CLS_CategoryEditDialog CategoryEditDialog { get; } = new CLS_CategoryEditDialog();
        public static CLS_RUN RUN { get; } = new CLS_RUN();
        public static CLS_ContentBrowserTop ContentBrowserTop { get; } = new CLS_ContentBrowserTop();
        public static CLS_ContentBrowserIdle ContentBrowserIdle { get; } = new CLS_ContentBrowserIdle();
        public static CLS_ContentBrowserLoading ContentBrowserLoading { get; } = new CLS_ContentBrowserLoading();
        public static CLS_ImagePreviewTop ImagePreviewTop { get; } = new CLS_ImagePreviewTop();
        public static CLS_IDLE IDLE { get; } = new CLS_IDLE();
        public static CLS_LOADING LOADING { get; } = new CLS_LOADING();
        public static CLS_LOADING_PREVIEW LOADING_PREVIEW { get; } = new CLS_LOADING_PREVIEW();
        public static CLS_LOADING_CONTENTLIST LOADING_CONTENTLIST { get; } = new CLS_LOADING_CONTENTLIST();
    }
    public partial class Events : WorkflowEventBase
    {
        public static CLS_ACT_SELECTED_ITEM ACT_SELECTED_ITEM { get; } = new CLS_ACT_SELECTED_ITEM();
        public static CLS_ACT_SAVE_CATEGORY_ONCATEGORYEDIT ACT_SAVE_CATEGORY_ONCATEGORYEDIT { get; } = new CLS_ACT_SAVE_CATEGORY_ONCATEGORYEDIT();
        public static CLS_TRNS_TOPSCREEN TRNS_TOPSCREEN { get; } = new CLS_TRNS_TOPSCREEN();
        public static CLS_TRNS_EXIT TRNS_EXIT { get; } = new CLS_TRNS_EXIT();
        public static CLS_TRNS_CategoryTreeLoading TRNS_CategoryTreeLoading { get; } = new CLS_TRNS_CategoryTreeLoading();
        public static CLS_TRNS_CategoryTreeIdle TRNS_CategoryTreeIdle { get; } = new CLS_TRNS_CategoryTreeIdle();
        public static CLS_TRNS_DISP_CATEGORYEDIT TRNS_DISP_CATEGORYEDIT { get; } = new CLS_TRNS_DISP_CATEGORYEDIT();
        public static CLS_TRNS_IDLE TRNS_IDLE { get; } = new CLS_TRNS_IDLE();
        public static CLS_ACT_SHOW_NAVIPREVIEW ACT_SHOW_NAVIPREVIEW { get; } = new CLS_ACT_SHOW_NAVIPREVIEW();
        public static CLS_ACT_SHOW_SELECTEDPREVIEW ACT_SHOW_SELECTEDPREVIEW { get; } = new CLS_ACT_SHOW_SELECTEDPREVIEW();
        public static CLS_TRNS_LOADING TRNS_LOADING { get; } = new CLS_TRNS_LOADING();
        public static CLS_ACT_SHOW_NAVINEXT_PREVIEW ACT_SHOW_NAVINEXT_PREVIEW { get; } = new CLS_ACT_SHOW_NAVINEXT_PREVIEW();
        public static CLS_ACT_SHOW_NAVIPREV_PREVIEW ACT_SHOW_NAVIPREV_PREVIEW { get; } = new CLS_ACT_SHOW_NAVIPREV_PREVIEW();
        public static CLS_ACT_CONNECTING_SERVICE ACT_CONNECTING_SERVICE { get; } = new CLS_ACT_CONNECTING_SERVICE();
        public static CLS_ACT_GETARTIFACTLIST ACT_GETARTIFACTLIST { get; } = new CLS_ACT_GETARTIFACTLIST();
        public static CLS_ACT_LOAD_PREVIEW ACT_LOAD_PREVIEW { get; } = new CLS_ACT_LOAD_PREVIEW();
        public static CLS_ACT_LOAD_CATEGORY ACT_LOAD_CATEGORY { get; } = new CLS_ACT_LOAD_CATEGORY();
        public static CLS_ACT_PUT_CATEGORY ACT_PUT_CATEGORY { get; } = new CLS_ACT_PUT_CATEGORY();
        public static CLS_ACT_POST_CATEGORY ACT_POST_CATEGORY { get; } = new CLS_ACT_POST_CATEGORY();
        public static CLS_ACT_GET_CATEGORY ACT_GET_CATEGORY { get; } = new CLS_ACT_GET_CATEGORY();
        public static CLS_ACT_FETCH_CATEGORY ACT_FETCH_CATEGORY { get; } = new CLS_ACT_FETCH_CATEGORY();
        public static CLS_TRNS_CONNECTED TRNS_CONNECTED { get; } = new CLS_TRNS_CONNECTED();
        public static CLS_TRNS_DISCONNECTED TRNS_DISCONNECTED { get; } = new CLS_TRNS_DISCONNECTED();
        public static CLS_ACT_SAVE_CATEGORY ACT_SAVE_CATEGORY { get; } = new CLS_ACT_SAVE_CATEGORY();
        public static CLS_TRNS_RUN TRNS_RUN { get; } = new CLS_TRNS_RUN();
        public static CLS_TRNS_LOADING_PREVIEW TRNS_LOADING_PREVIEW { get; } = new CLS_TRNS_LOADING_PREVIEW();
        public static CLS_TRNS_INIT TRNS_INIT { get; } = new CLS_TRNS_INIT();
        public static CLS_TRNS_LOADING_CONTENTLIST TRNS_LOADING_CONTENTLIST { get; } = new CLS_TRNS_LOADING_CONTENTLIST();
        public static CLS_ACT_LOAD_CATEGORYLIST ACT_LOAD_CATEGORYLIST { get; } = new CLS_ACT_LOAD_CATEGORYLIST();
        public static CLSINVALID_INVALID INVALID { get; } = new CLSINVALID_INVALID();
    }
}
