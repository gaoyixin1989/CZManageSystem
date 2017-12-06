using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Botwave.Web;
using Botwave.Workflow;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;
using Botwave.XQP.Service.Plugins;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.GMCCServiceHelpers;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Practices.CZMCC.Support;

public partial class contrib_workflow_controls_ReviewHandleSelector : Botwave.XQP.Web.Controls.WorkflowReviewSelector
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_controls_ReviewHandleSelector));

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
