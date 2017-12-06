using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Service;
using Botwave.Security.Web;
using Botwave.Web;
using Spring.Context.Support;

public partial class contrib_workflow_controls_ProcessHistoryEditorLoader : Botwave.Web.UserControlBase
{
    public const string DefaultControlPath = "~/apps/xqp2/pages/workflows/maintenance/ProcessHistoryEditor.ascx";
    private IDictionary<string, string> historyControlTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    private IWorkflowMaintenanceService workflowMaintenanceService = (IWorkflowMaintenanceService)ContextRegistry.GetContext().GetObject("workflowMaintenanceService");

    public IWorkflowMaintenanceService WorkflowMaintenanceService
    {
        set { workflowMaintenanceService = value; }
    }
    /// <summary>
    /// key:流程名称, value:空间路径.
    /// </summary>
    public System.Collections.Specialized.HybridDictionary HistoryControls
    {
        set
        {
            if (value != null && value.Count > 0)
            {
                foreach (object key in value.Keys)
                {
                    string itemKey = key.ToString();
                    string itemValue = value[key].ToString();
                    if (!string.IsNullOrEmpty(itemValue))
                    {
                        historyControlTypes[itemKey] = itemValue;
                    }
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void OnBind(Guid workflowInstanceId)
    {
        this.OnBind(string.Empty, workflowInstanceId);
    }

    public void OnBind(string workflowName, Guid workflowInstanceId)
    {
        string controlVirtualPath = DefaultControlPath;
        Botwave.XQP.Web.Controls.WorkflowProcessHistoryEditor control = this.LoadControl(controlVirtualPath) as Botwave.XQP.Web.Controls.WorkflowProcessHistoryEditor;
        
        this.historyHolder.Controls.Add(control);
        control.Initialize(string.Empty, workflowInstanceId);
    }

    protected void btnDelOption_Click(object sender, EventArgs e)
    {
        string wiid = Request.QueryString["wiid"];
        workflowMaintenanceService.WorkflowProcessHistoryDelete(new Guid(wiid), this.Request);
        LogWriterFactory.Writer.WriteNomalLog("admin", "保存工单", "删除工单处理记录" + wiid + "内容成功");
        ShowSuccess(MessageHelper.Message_Success, string.Format(AppPath + "apps/xqp2/pages/workflows/maintenance/process.aspx?wiid={0}", wiid));
    }
}
