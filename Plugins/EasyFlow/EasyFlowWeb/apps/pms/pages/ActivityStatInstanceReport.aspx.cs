using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Botwave.Extension.IBatisNet;
using Botwave.Security;
using Botwave.XQP.Service;
using Botwave.XQP.Util;
using Botwave.XQP.Domain;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Util;
using System.Text.RegularExpressions;

public partial class apps_xqp2_pages_workflows_report_ActivityStatInstanceReport : DrillBase
{
    private string advanceResource = "A019";

    private IUcs_ReportformsService ucs_ReportformsService = (IUcs_ReportformsService)Ctx.GetObject("ucs_ReportformsService");
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");

    protected IUcs_ReportformsService Ucs_ReportformsService
    {
        get { return ucs_ReportformsService; }
        set { ucs_ReportformsService = value; }
    }

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    /// <summary>
    /// 高级权限资源.
    /// </summary>
    public string AdvanceResource
    {
        get { return advanceResource; }
        set { advanceResource = value; }
    }

    public bool EnableAdvanceReport
    {
        get { return ViewState["EnableAdvanceReport"] == null ? false : (bool)ViewState["EnableAdvanceReport"]; }
        set { ViewState["EnableAdvanceReport"] = value; }
    }

    public string Workflows
    {
        get { return (ViewState["Workflows"] == null ? string.Empty : (string)ViewState["Workflows"]); }
        set { ViewState["Workflows"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid id = new Guid(Request.QueryString["formid"]);
        reportform = ucs_ReportformsService.GetReprotformsByid(id, CurrentUser.UserId);
        if (id == new Guid("18300f17-d97d-4fe1-80d4-253124c697b2"))
            con_Pms.ShowCheckBox();
        con_Pms.reportforms = reportform;
        Title = reportform.formname;

        con_Pms.Seach += new apps_pms_pages_Ucs_Serch.DelegateSeachHander(con_Pms_Seach);

            Field = GetField(reportform.FieldList);
        if (!IsPostBack)
        {
            this.EnableAdvanceReport = HasAdvanceReport(CurrentUser);
            if (!string.IsNullOrEmpty(Request.QueryString["RealName"]) || !string.IsNullOrEmpty(Request.QueryString["DpFullName"]))
            {
                //将查询条件存入session中，用于页面跳转时在另外一个页面获取前一个页面的查询条件
                WhereDict = Session["wheredict"] != null ? (Dictionary<string, string>)Session["wheredict"] : new Dictionary<string, string>();
                for (var i = 0; i < Request.QueryString.Count; i++)
                {
                    Regex rx = new Regex(" as ");
                    var obj = reportform.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).Trim().Trim('\'') == Request.QueryString.Keys[i].ToLower().Trim());
                    if (obj.Count() > 0)
                    {
                        if (WhereDict.ContainsKey(Request.QueryString.Keys[i]))
                            WhereDict[Request.QueryString.Keys[i]] = Request.QueryString[Request.QueryString.Keys[i]];
                        else
                            WhereDict.Add(Request.QueryString.Keys[i], Request.QueryString[Request.QueryString.Keys[i]]);
                    }
                    else
                        continue;
                }
                Session["wheredict"] = WhereDict;
                Bind(pageSize, pageIndex);
                //ltlScript.Text = "<script>    search();    function search() { encodeStr(); document.getElementById('ctl00_cphBody_con_Pms_btn_Seach').click() }</script>";
                //ltlScript.Text = string.Empty;
            }
            else
            {
                Session["wherestr"] = null;
                Session["wheredict"] = null;
            }
        }
    }
    private string GetWhereStr()
    {
        StringBuilder whereStr = new StringBuilder();
        whereStr.Append("(1=1 ");
        
        
        //判断是不是系统管理员
        if (!this.EnableAdvanceReport && !string.IsNullOrEmpty(reportform.FilterRule) && reportform.type == 3)
        {
            whereStr.Append(" and ").Append(reportform.FilterRule.Replace("#UserName#", "'"+CurrentUser.UserName+"'")).Replace("#Workflows#", "'" + GetAllowedWorkflows("0002") + "'");
        }
        GetWhere(whereStr);
        whereStr.Append(") ");

        whereStr.Append(" or (startedtime is null ");
        if (!string.IsNullOrEmpty(Request.Form["WorkflowAlias"]))
        {
            whereStr.Append("and WorkflowAlias in ('" + DecodeBase64("utf-8", Request.Form["WorkflowAlias"]).Replace(",", "','") + "') ");
        }
        if (!string.IsNullOrEmpty(Request.Form["WorkflowName"]))
        {
            whereStr.Append("and WorkflowName in ('" + DecodeBase64("utf-8", Request.Form["WorkflowName"]).Replace(",", "','") + "') ");
        }
        whereStr.Append(")");
        return whereStr.ToString();
    }




    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        Bind(listPager.ItemsPerPage, e.NewPageIndex);
    }
    string lvl;
    private void con_Pms_Seach(object serder, EventArgs e)
    {
        WhereDict = new Dictionary<string, string>();
        StringBuilder sb = new StringBuilder();
        foreach (var item in reportform.FieldList)
        {
            string fieldval = item.whereFieldValue == null ? "" : (item.whereFieldValue.Substring((item.whereFieldValue.ToLower().IndexOf(" as") > 0 ? item.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim());
            if (item.whereStrtype == 1 && !string.IsNullOrEmpty(Request.Form[fieldval]) || item.Fieldtype > 1)
                switch (item.Fieldtype)
                {
                    case 1:

                        StringBuilder sb2 = new StringBuilder();
                        sb2.Append(" and ").Append(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)).Append(" in ('").Append(DecodeBase64("utf-8", Request.Form[fieldval]).Replace("&gt;", ">").Replace(",", "','")).Append("')");
                        if (item.field.ToLower().Contains("sum("))
                        {
                            groupStr += sb2.ToString();
                        }
                        else
                        {
                            if (!WhereDict.ContainsKey(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)))
                                WhereDict.Add(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length), DecodeBase64("utf-8", Request.Form[fieldval]).Replace("&gt;", ">"));
                            else
                                WhereDict[item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)] = DecodeBase64("utf-8", Request.Form[fieldval]).Replace("&gt;", ">");
                            sb.Append(sb2.ToString());
                        }
                        break;
                    case 7:
                        if (!string.IsNullOrEmpty(Request.Form[item.whereFieldValue + "1"]))
                        {
                            if (!WhereDict.ContainsKey(item.field + "1"))
                                WhereDict.Add(item.field + "1", Request.Form[item.whereFieldValue + "1"]);
                            else
                                WhereDict[item.field + "1"] = Request.Form[item.whereFieldValue + "1"];
                            sb.Append(" and ").Append(item.field).Append(" >=convert(datetime,'").Append(Request.Form[item.whereFieldValue + "1"]).Append("')");
                        }
                        if (!string.IsNullOrEmpty(Request.Form[item.whereFieldValue + "2"]))
                        {
                            if (!WhereDict.ContainsKey(item.field + "2"))
                                WhereDict.Add(item.field + "2", Request.Form[item.whereFieldValue + "2"]);
                            else
                                WhereDict[item.field + "2"] = Request.Form[item.whereFieldValue + "2"];
                            sb.Append(" and ").Append(item.field).Append(" <=convert(datetime,'").Append(Request.Form[item.whereFieldValue + "2"]).Append("')");
                        } break;
                }
        }
        if (sb.Length > 0)//将查询条件存入session，用于页面跳转时在另外一个页面获取前一个页面的查询条件
        {
            Session["wherestr"] = sb.ToString();
            Session["wheredict"] = WhereDict;
        }
        listPager.CurrentPageIndex = 0;
        listPager.DataBind();
        Bind(0, 0);
    }

    string groupStr = "";
    protected StringBuilder javascript=new StringBuilder();
    protected string imgstr = "";
    private void Bind(int pageSize, int pageIndex)
    {

        
        if (con_Pms.IsChecked())
        {
            reportform.strGroup += ",ActorDescription";
            reportform.FieldList.Where(x => x.field.ToLower() == "actordescription").First().IsSelect = true;
            reportform.FieldList.OrderBy(x=>x.fieldorder);
            Field = GetField(reportform.FieldList); 
        }
        groupStr = reportform.strGroup;
        string field = GetFieldStr();
        string strWhere = GetWhereStr();
        string fieldtext = GetFieldText(reportform.FieldList.Where(x => !string.IsNullOrEmpty(x.fieldname)).ToList());
        dt = CommontUnit.Instance.GetUsersByPager(reportform.datasource, "x", field.Replace('|', ','), strWhere, reportform.strOrder, pageIndex, listPager.ItemsPerPage, groupStr, ref total);
        // dataJOSN = JsonHelper.ConvertDateTableToJson(dt);

        fromHtml = ucs_ReportformsService.GetTableHtml(dt, reportform.type, fieldtext, reportform, lvl, "");
        //string imgid=IBatisDbHelper.ExecuteScalar(CommandType.Text, "select imgformid from cz_WorkflowInterfaces where id="+Request.QueryString["menuid"]).ToString();
        //if (!string.IsNullOrEmpty(imgid) && imgid != "00000000-0000-0000-0000-000000000000")
        //{
        //    var imgReprot = ucs_ReportformsService.GetReprotformsByid(new Guid(imgid), CurrentUser.UserId);
        //    imgstr = ucs_ReportformsService.GetImgHtml(imgReprot, javascript, strWhere, lvl);

        //} 
        listPager.TotalRecordCount = total;
        listPager.DataSource = dt;
        listPager.DataBind();
        exp_Pms.ExplortData = dt;
        exp_Pms.TableName = reportform.datasource;
        exp_Pms.GetDataWhere = strWhere;
        exp_Pms.ExplortFilter = field;
        exp_Pms.ZHFilter = fieldtext;
        exp_Pms.TotoalCount = total;
        exp_Pms.ExportType = 0;
        exp_Pms.KeyFilter = "x";
        exp_Pms.ByGroup = reportform.strGroup;
        exp_Pms.GroupType = 0;
        exp_Pms.ByOrder = reportform.strOrder;
        exp_Pms.Code = string.Empty;

    }

    private string GetFieldStr()
    {
        switch (reportform.type)
        {
            case 1: return GetField(reportform.FieldList);
            case 4:
            case 5:
            case 3: return Field.Replace("GROUP_VEST_LVL_1", lvl);
            default: return Field;
        }
    }


    private void GetWhere(StringBuilder sb)
    {
        IDictionary<string, string> whereCondition = Session["wheredict"] != null ? (Dictionary<string, string>)Session["wheredict"] : new Dictionary<string, string>();//获取Session中的条件
        foreach (var item in reportform.FieldList)
        {
            string fieldval = item.whereFieldValue == null ? "" : (item.whereFieldValue.Substring((item.whereFieldValue.ToLower().IndexOf(" as") > 0 ? item.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim());
            if (item.whereStrtype == 1 && (!string.IsNullOrEmpty(Request.Form[fieldval]) || item.Fieldtype > 1 || whereCondition.ContainsKey(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length))))
                switch (item.Fieldtype)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 6:
                    case 8:
                        StringBuilder sb2 = new StringBuilder();
                        if (whereCondition.ContainsKey(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)))
                            sb2.Append(" and ").Append(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)).Append(" in ('").Append(Server.HtmlDecode(whereCondition[item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)])).Replace("&gt;", ">").Replace(",","','").Append("')");
                        else
                            sb2.Append(" and ").Append(item.field.Substring(0, item.field.ToLower().LastIndexOf(" as") > 0 ? item.field.ToLower().LastIndexOf(" as") : item.field.Length)).Append(" in ('").Append(Server.HtmlDecode(DecodeBase64("utf-8", Request.Form[fieldval])).Replace("&gt;", ">").Replace(",", "','")).Append("')");
                        if (item.field.ToLower().Contains("sum("))
                        {
                            groupStr += sb2.ToString();
                        }
                        else
                        {
                            sb.Append(sb2.ToString());
                        }
                        break;
                    case 7:
                        if (whereCondition.ContainsKey(item.field + "1"))
                        {
                            sb.Append(" and ").Append(item.field).Append(" >=convert(datetime,'").Append(whereCondition[item.field + "1"]).Append("')");
                        }
                        else if (!string.IsNullOrEmpty(Request.Form[item.whereFieldValue + "1"]))
                        {
                            sb.Append(" and ").Append(item.field).Append(" >=convert(datetime,'").Append(Request.Form[item.whereFieldValue + "1"]).Append("')");
                        }
                        if (whereCondition.ContainsKey(item.field + "2"))
                        {
                            sb.Append(" and ").Append(item.field).Append(" <=convert(datetime,'").Append(whereCondition[item.field + "2"]).Append("')");
                        }
                        else if (!string.IsNullOrEmpty(Request.Form[item.whereFieldValue + "2"]))
                        {
                            sb.Append(" and ").Append(item.field).Append(" <=convert(datetime,'").Append(Request.Form[item.whereFieldValue + "2"]).Append("')");
                        } break;
                }
        }
        for (var i = 0; i < Request.QueryString.Count; i++)
        {
            UCS_FromField field = null;
            Regex rx = new Regex(" as ");
            var obj = reportform.FieldList.Where(x => (x.field.ToLower().IndexOf(" as") > 0 ? rx.Split(x.field.ToLower())[rx.Split(x.field.ToLower()).Length - 1] : x.field.ToLower()).Trim().Trim('\'') == Request.QueryString.Keys[i].ToLower().Trim());
            if (obj.Count() > 0)
            {
                field = obj.First();
            }
            else
                continue;
            string fieldval = field.whereFieldValue == null ? "" : (field.whereFieldValue.Substring((field.whereFieldValue.ToLower().IndexOf(" as") > 0 ? field.whereFieldValue.ToLower().IndexOf(" as") : -3) + 3).Trim());
            if (Request.QueryString[Request.QueryString.Keys[i]] != null && Request.Form[fieldval] == null)
            {
                //WhereDict = Session["wheredict"] == null ? new Dictionary<string, string>() : (Dictionary<string, string>)Session["wheredict"];
                //if (!WhereDict.ContainsKey(Request.QueryString.Keys[i]))
                //    WhereDict.Add(Request.QueryString.Keys[i], Request.QueryString[Request.QueryString.Keys[i]]);
                //else
                //    WhereDict[Request.QueryString.Keys[i]] = Request.QueryString[Request.QueryString.Keys[i]];
                //Session["wheredict"] = WhereDict;
                sb.Append(" and ").Append(Request.QueryString.Keys[i]).Append("='").Append(Server.HtmlDecode(Request.QueryString[i])).Append("'");
            }
        }
        if (reportform.FieldList.Where(x => x.Fieldtype == 6 || x.Fieldtype == 7).Count() > 0)
        {
            //sb.Append(" and TM_INTRVL_CD =").Append(string.IsNullOrEmpty(Request.Form["TM_INTRVL_CD"]) ? GetMaxMonth(reportform.datasource) : Request.Form["TM_INTRVL_CD"]);
        }
    }

    private string GetAllowedWorkflows(string postfixs)
    {
        IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
        //if (workflows != null && workflows.Count > 0 
        //    && postfixs != null && postfixs.Length > 0)
        //workflows = WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, postfixs);
        StringBuilder sb = new StringBuilder();
        if (!this.EnableAdvanceReport)
        {
            if (workflows != null && workflows.Count > 0
                && postfixs != null && postfixs.Length > 0)
            {
                IList<WorkflowDefinition> allowWorkflows = WorkflowUtility.GetAllowedWorkflows(workflows, CurrentUser.Resources, postfixs);
                
                foreach (WorkflowDefinition item in allowWorkflows)
                {
                    sb.Append(item.WorkflowName + ",");
                }
                if (sb.Length > 0)
                {
                    sb = sb.Remove(sb.Length - 1, 1).Replace(",", "','");
                    //Workflows = sb.ToString();
                }
            }
        }
        return sb.ToString();
    }

    protected bool HasAdvanceReport(Botwave.Security.LoginUser user)
    {
        if (user == null)
            return false;
        if (string.IsNullOrEmpty(this.advanceResource))
            return true;

        bool result = false;
        if (user.Properties.ContainsKey("Report_Advance_Enable"))
        {
            result = Convert.ToBoolean(user.Properties["Report_Advance_Enable"]);
        }
        else
        {
            result = user.Resources.ContainsKey(this.advanceResource);
            user.Properties["Report_Advance_Enable"] = result;
        }
        return result;
    }
}