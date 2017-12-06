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

using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_help_controls_helptree : Botwave.Security.Web.UserControlBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindHelpTree();
        }
    }

    /// <summary>
    /// 绑定部门树结构.
    /// </summary>
    private void BindHelpTree()
    {
        DataView source = Help.GetHelpList().DefaultView;
        this.Recursive(source, 0, treehelp.Nodes);
    }

    private void Recursive(DataView sourceData, int parentId, TreeNodeCollection treeNodes)
    {
        DataView data = new DataView(sourceData.Table, string.Format("ParentId = {0}", parentId), "", DataViewRowState.CurrentRows);
        TreeNode node;
        int count = data.Count;
        for (int i = 0; i < count; i++)
        {
            node = new TreeNode();

            int Id = Botwave.Commons.DbUtils.ToInt32(data[i]["Id"]);
                node.Expanded = true;
            node.Text = Botwave.Commons.DbUtils.ToString(data[i]["Title"]);
            node.Value = Id.ToString();
            node.NavigateUrl = string.Format("javascript:onTreeNodeClick('{0}');", Id);
            treeNodes.Add(node);
            this.Recursive(sourceData, Id, node.ChildNodes);
        }
    }
}
