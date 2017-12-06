using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_pages_PopupUserPicker :  Botwave.Security.Web.PageBase
{
    protected IUserService userService = (IUserService)Ctx.GetObject("userService");
    protected IDepartmentService departmentService = (IDepartmentService)Ctx.GetObject("departmentService");
    private string rootDeptId = "63A107F1CC13452D9349CA6F45A49DCD";
    private string expandDeptId = "63A107F1CC13452D9349CA6F45A49DCD";
    private string rootDeptName = "潮州分公司";

    public IUserService UserService
    {
        get { return userService; }
        set { userService = value; }
    }

    public IDepartmentService DepartmentService
    {
        get { return departmentService; }
        set { departmentService = value; }
    }

    /// <summary>
    /// 显示树的根部门编号.
    /// </summary>
    public string RootDeptId
    {
        set { rootDeptId = value; }
    }

    /// <summary>
    /// 展开显示的部门编号.
    /// </summary>
    public string ExpandDeptId
    {
        set { expandDeptId = value; }
    }

    /// <summary>
    /// 根结点名称.
    /// </summary>
    public string RootNodeName
    {
        set { rootDeptName = value; }
    }

    public string OpenerInputId
    {
        get { return (string)ViewState["OpenerInputId"]; }
        set { ViewState["OpenerInputId"] = value; }
    }

    /// <summary>
    /// 用户部门编号.
    /// </summary>
    public string UserDpId
    {
        get { return ViewState["UserDpId"] == null ? string.Empty : (string)ViewState["UserDpId"]; }
        set { ViewState["UserDpId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.Expires = 0;
        this.Response.CacheControl = "no-cache";
        this.Response.AddHeader("Pragma", "No-Cache");
        this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        if (!IsPostBack)
        {
            Botwave.Security.LoginUser user = CurrentUser;
            string userDpId = user.DpId;
            this.UserDpId = userDpId;
            this.SearchUsers(string.IsNullOrEmpty(userDpId) ? string.Empty : userDpId, 0, 0);

            this.BindDepartmentTree();

            string inputId = Server.HtmlEncode(Request.QueryString["inputId"]);
            this.OpenerInputId = inputId;
        }
    }

    /// <summary>
    /// 绑定部门树结构.
    /// </summary>
    private void BindDepartmentTree()
    {
        DataView source = Botwave.XQP.Commons.XQPHelper.GetAllDepartments().DefaultView; //departmentService.GetAllDepartments().DefaultView;
        //TreeNode node = new TreeNode(this.rootDeptName);
        //node.Value = this.rootDeptId;
        //this.treeDepts.Nodes.Add(node);
        //this.RecursiveDepartmets(source, this.rootDeptId, node);  // 直接获取"广州移动通信公司"以下部门
        this.RecursiveDepartmets(source, "85D176CCBC8942F3B54DE050BE748A58", null);  // 直接获取"广州移动通信公司"以下部门
    }

    private void RecursiveDepartmets(DataView sourceData, string parentId, TreeNode parentNode)
    {
        DataView data = new DataView();
        if (string.IsNullOrEmpty(parentId))
            data = new DataView(sourceData.Table, string.Format("ParentDpId is null or ParentDpId = ''"), "", DataViewRowState.CurrentRows);
        else
            data = new DataView(sourceData.Table, string.Format("ParentDpId = '{0}'", parentId), "", DataViewRowState.CurrentRows);
        TreeNode node;
        int count = data.Count;
        for (int i = 0; i < count; i++)
        {
            int level = Botwave.Commons.DbUtils.ToInt32(data[i]["DpLevel"]);
            if (level > 6)
                break;
            node = new TreeNode();
            if (level > 2)
                node.Expanded = false;

            string dpId = Botwave.Commons.DbUtils.ToString(data[i]["DpId"]).Trim();
            if (dpId == this.expandDeptId)
            {
                node.Expanded = true;
            }
            if (dpId == this.UserDpId)
            {
                if (parentNode != null)
                    parentNode.Expanded = true;
                node.Expanded = true;
                node.Checked = true;
            }
            node.Text = Botwave.Commons.DbUtils.ToString(data[i]["DpName"]);
            node.Value = dpId;
            //node.NavigateUrl = string.Format("javascript:return onTreeNodeClick('{0}');", dpId);
            if (parentNode == null)
                this.treeDepts.Nodes.Add(node);
            else
                parentNode.ChildNodes.Add(node);
            this.RecursiveDepartmets(sourceData, dpId, node);
        }
    }

    #region 部门数据树结构和用户列表

    protected void treeDepts_SelectedNodeChanged(object sender, EventArgs e)
    {
        listPager.CurrentPageIndex = 0;
        string dpId = string.Empty;
        if (this.treeDepts.SelectedNode != null)
        {
            dpId = this.treeDepts.SelectedNode.Value;
            //dpId = (this.treeDepts.SelectedNode.Parent == null ? string.Empty : dpId);
        }
        SearchUsers(dpId, 0, 0);
    }

    private void SearchUsers(string dpId, int recordCount, int pageIndex)
    {
        string keywords = this.txtKeyword.Text;
        if (!string.IsNullOrEmpty(keywords)) dpId = "";
         //DataTable results = userService.GetUsersByPager(keywords, dpId, pageIndex, listPager.ItemsPerPage, ref recordCount);
         DataTable results = Botwave.Workflow.Practices.CZMCC.Support.UserService.Current.GetUsersByPager(keywords, dpId, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.rptUsers.DataSource = results;
        this.rptUsers.DataBind();

        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        string dpId = this.UserDpId;
        if (this.treeDepts.SelectedNode != null)
        {
            dpId = this.treeDepts.SelectedNode.Value;
        }
        this.SearchUsers(dpId, listPager.TotalRecordCount, e.NewPageIndex);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        listPager.CurrentPageIndex = 0;
        string dpId = string.Empty;
        if (this.treeDepts.SelectedNode != null)
        {
            dpId = this.treeDepts.SelectedNode.Value;
        }
        SearchUsers(dpId, 0, 0);
    }

    #endregion
}
