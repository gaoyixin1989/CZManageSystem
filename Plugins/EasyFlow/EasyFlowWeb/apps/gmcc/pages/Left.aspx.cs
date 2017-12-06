using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using Botwave.Extension.IBatisNet;

public partial class apps_gmcc_Left : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string GetWorkflows()
    {

        string sql = string.Format(@"select bw.[WorkflowId],g.[WorkflowName],[WorkflowAlias],[AliasImage] as AliasImageUrl from [bwwf_WorkflowSettings] g,[bwwf_Workflows] bw 
where g.[WorkflowName]=bw.[WorkflowName] and bw.[IsCurrent]=1 and enabled=1 and isdeleted=0 order by bw.[CreatedTime] asc");

        DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        int n = dt.Rows.Count;
        string resultValue = string.Empty;
        StringBuilder result = new StringBuilder();
        result.Append("[");
        if (n > 0)
        {
            for (int i = 0; i < n; i++)
            {
                //kamael by 2013-01-09
                //string requestResourceId = GetWorlkowRequestResourceId(dt.Rows[i]["WorkflowName"].ToString().Trim());
                //if (!CurrentUser.Resources.ContainsKey(requestResourceId)) continue;
                string img = string.Empty;
                if (dt.Rows[i]["AliasImageUrl"] != null && dt.Rows[i]["AliasImageUrl"].ToString().Trim() != "")
                {
                    img = string.Format("<img alt='{0}' class='groupImage' src='{1}' />", dt.Rows[i]["WorkflowAlias"].ToString(), AppPath + "contrib/workflow/res/groups/"+dt.Rows[i]["AliasImageUrl"].ToString());
                }
                else
                {
                    img = string.Format("[{0}]", dt.Rows[i]["WorkflowAlias"].ToString());
                }

                string href = string.Format("{3}<a href='{0}apps/xqp2/pages/workflows/WorkflowIndex.aspx?wid={1}' target='rightFrame' bwtip='flowmenu' id='{1}' params='title={2}&apppath={0}&width=170' >{2}</a>",
                      AppPath, dt.Rows[i]["WorkflowId"].ToString(), dt.Rows[i]["WorkflowName"].ToString(), img);

                result.Append("{");
                result.Append(string.Format("name:\"{0}{1}\" ,to:\"{2}\"", dt.Rows[i]["WorkflowAlias"].ToString(), dt.Rows[i]["WorkflowName"].ToString(), href));
                if (i != n - 1)
                {
                    result.Append("},");
                }
                else
                {
                    result.Append("}");
                }
            }
            result.Append("]");
            resultValue = result.ToString();
            int length = result.ToString().Length;
            if (length > 4)
            {
                string charIndex = result.ToString().Substring(length - 2, 1);
                if (charIndex == ",")
                {
                    resultValue = result.ToString().Substring(0, length - 2) + "]";
                }
            }
            return resultValue;

        }
        return "";
    }
}
