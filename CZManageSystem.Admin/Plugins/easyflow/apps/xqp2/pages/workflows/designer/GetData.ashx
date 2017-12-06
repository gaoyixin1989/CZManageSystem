<%@ WebHandler Language="C#" Class="GetDepartments" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;
using System.Xml;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.Extension.IBatisNet;

public class GetDepartments : IHttpHandler {

    private IDepartmentService departmentService = Spring.Context.Support.WebApplicationContext.Current["departmentService"] as IDepartmentService;
    private IUserService userService = Spring.Context.Support.WebApplicationContext.Current["userService"] as IUserService;
    private IWorkflowFieldService workflowFieldService = new Botwave.XQP.Service.Plugins.WorkflowFieldService();
    
    private const int PageSize = 20;    
    
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/xml";
        context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        string queryType = context.Request.QueryString["type"];
        string queryPageIndex = context.Request.QueryString["page"];
        string queryParentDpId = context.Request.QueryString["parentId"];
        string queryWorkflowId = context.Request.QueryString["workflowId"];
        string queryFieldName = context.Request.QueryString["field"];
        string queryActivityName = context.Request.QueryString["activity"];
        
        queryType = string.IsNullOrEmpty(queryType) ? string.Empty : queryType.Trim().ToLower();
        
        string result = "<none />";
        if (queryType == "user")
        {
            int pageIndex = string.IsNullOrEmpty(queryPageIndex) ? 0 : Convert.ToInt32(queryPageIndex);
            result = BindActors("", pageIndex);
        }
        else if (queryType == "department")
        {
            string parentDpId = string.IsNullOrEmpty(queryParentDpId) ? "34920440002" : queryParentDpId;
            result = BindDepartments(parentDpId);
        }
        else if (queryType == "field")
        {
            Guid? workflowId = string.IsNullOrEmpty(queryWorkflowId) ?  (new  Nullable<Guid>()) : new Guid(queryWorkflowId);
            if (workflowId.HasValue)
                result = BindFileds(workflowId.Value);
            else
                result = "<fileds />";
        }
        else if (queryType == "fieldvalue")
        {
            Guid? workflowId = string.IsNullOrEmpty(queryWorkflowId) ? (new Nullable<Guid>()) : new Guid(queryWorkflowId);
            if (workflowId.HasValue)
                result = BindFieldControls(workflowId.Value, queryActivityName, queryFieldName);
            else
                result = "<fieldValues />";
        }
        context.Response.Write(result);
    }

    private string BindActors(string dpId, int pageIndex)
    {
        int count = 0;
        StringBuilder builder = new StringBuilder();
        DataTable results = userService.GetUsersByPager(string.Empty, dpId, pageIndex, PageSize, ref count);
        builder.AppendFormat("<users count=\"{0}\">", count);
        foreach (DataRow row in results.Rows)
        {
            builder.AppendFormat("<item id=\"{0}\" name=\"{1}\" department=\"{2}\" />\r\n", row["UserName"].ToString(), row["realName"].ToString(), row["DpFullName"].ToString());
        }
        builder.Append("</users>");
        return builder.ToString();
    }

    private string BindDepartments(string parentDpId)
    {
        IList<Department> results = departmentService.GetDepartmentsByParentId(parentDpId);

        StringBuilder builder = new StringBuilder("<departments>");
        if(results != null  && results.Count>0)
        {
            foreach (Department item in results)
            {
                builder.AppendFormat("<item id=\"{0}\" name=\"{1}\" fullName=\"{2}\" />\r\n", item.DpId.Trim(), item.DpName, item.DpFullName);
            }
        }
        
        builder.Append("</departments>");
        return builder.ToString();
    }

    private string BindFileds(Guid workflowId)
    {
        StringBuilder builder = new StringBuilder("<fileds>");
        IList<FieldInfo> results = workflowFieldService.GetControllableFields(workflowId);
        foreach (FieldInfo item in results)
        {
            builder.AppendFormat("<item name=\"{0}\" headerText=\"{1}\" display=\"{2}\" />\r\n", item.FieldName, item.HeaderText, item.DisplayName);
        }
        builder.Append("</fileds>");
        return builder.ToString();
    }

    private string BindFieldControls(Guid workflowId, string activityName, string fieldName)
    {
        StringBuilder builder = new StringBuilder("<fieldValues>");
        string workflowName = WorkflowUtility.GetWorkflowName(workflowId);
        
        if (!string.IsNullOrEmpty(workflowName) && !string.IsNullOrEmpty(activityName) && !string.IsNullOrEmpty(fieldName))
        {
            builder = new StringBuilder();
            builder.AppendFormat("<fieldValues workflow=\"{0}\" activity=\"{1}\" field=\"{2}\">\r\n", workflowName, activityName, fieldName);
            
            FieldInfo field = workflowFieldService.GetField(workflowId, fieldName);
            if (field != null)
            {
                IList<FieldControlInfo> fieldControls = workflowFieldService.GetFieldControls(workflowName, activityName, fieldName);
                IDictionary<string, int> dict = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
                foreach (FieldControlInfo item in fieldControls)
                {
                    if (!dict.ContainsKey(item.FieldValue))
                        dict.Add(item.FieldValue, item.Id);
                }
                IList<FieldControlInfo> emptyControls = field.ToEmptyFieldControls(workflowName, activityName);
                foreach (FieldControlInfo item in emptyControls)
                {
                    if (!dict.ContainsKey(item.FieldValue))
                        fieldControls.Add(item);
                }
                
                foreach (FieldControlInfo item in fieldControls)
                {
                    builder.AppendFormat("<item name=\"{0}\" value=\"{1}\" />\r\n", FormatXml(item.FieldValue), item.TargetUsers);
                }
            }
        }
        builder.Append("</fieldValues>");
        return builder.ToString();
    }

    private static string FormatXml(string text)
    {
        return text.Replace("&", "&amp;").Replace(">", "&lt;");
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
