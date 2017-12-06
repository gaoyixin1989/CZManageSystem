<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_mobile_ajax_doneTaskByApplAjax" Codebehind="doneTaskByApplAjax.aspx.cs" %>

 <%
    System.Data.DataTable dtResult = source;
    foreach(System.Data.DataRow row in source.Rows)
    {%>
    <a href="#" class="list-group-item ui-btn-icon-right ui-icon-carat-r">
                        <div style="float: left; margin-top: 3em">
                        <%if(!string.IsNullOrEmpty(Botwave.Commons.DbUtils.ToString(row["AliasImage"]))){%>
                           <img alt="<%=row["WorkflowAlias"] %>" class="groupImage" src="<%=AppPath %>contrib/workflow/res/groups/<%=row["AliasImage"] %>" />
                            <%}%>
                        </div>
                        <div style="margin-left: 2.8em;" onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='<%=AppPath%>contrib/mobile/pages/workflowview.aspx?wiid=<% =row["WorkflowInstanceId"]%>&returnurl=<%=HttpUtility.UrlEncode("doneTaskByAppl.aspx") %>'">
                            <h4 class="list-group-item-heading">
                                <%=row["title"] %></h4>
                            <p class="list-group-item-text">
                                受理号：<%=row["sheetid"] %></p>
                            <p class="list-group-item-text">
                                步&nbsp;&nbsp; 骤：<%=GetCurrentActNames(Botwave.Commons.DbUtils.ToString(row["workflowinstanceid"]),Convert.ToInt32(row["state"])) %></p>
                            <p class="list-group-item-text">
                                处理人： <%=GetCurrentActors(Botwave.Commons.DbUtils.ToString(row["workflowinstanceid"]), Convert.ToInt32(row["state"]))%></p>
                        </div>
                    </a>
    <%} 
    if (dtResult.Rows.Count == 0 && PageIndex == 0)
        {%>
            <div >
                    <h4 class="list-group-item-heading"><span class="pullDownLabel">无记录显示</span></h4>
                </div>
      <%}%>