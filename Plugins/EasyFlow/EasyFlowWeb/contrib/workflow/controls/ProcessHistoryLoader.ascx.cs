using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class contrib_workflow_controls_ProcessHistoryLoader : System.Web.UI.UserControl
{
    public const string DefaultControlPath = "~/plugins/easyflow/contrib/workflow/controls/ProcessHistory.ascx";//wbl
    private IDictionary<string, string> historyControlTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

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
        if (this.historyControlTypes.ContainsKey(workflowName))
            controlVirtualPath = historyControlTypes[workflowName];
        Botwave.XQP.Web.Controls.WorkflowProcessHistory control = this.LoadControl(controlVirtualPath) as Botwave.XQP.Web.Controls.WorkflowProcessHistory;
        
        this.historyHolder.Controls.Add(control);
        control.Initialize(string.Empty, workflowInstanceId);
    }
}
