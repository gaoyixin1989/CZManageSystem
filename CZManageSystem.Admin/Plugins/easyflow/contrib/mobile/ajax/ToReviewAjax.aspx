<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_mobile_ajax_ToReviewAjax" Codebehind="ToReviewAjax.aspx.cs" %>
    <%
    System.Data.DataTable dtResult = source;
    foreach(System.Data.DataRow row in source.Rows)
    {%>
    <div class="list-group-item ui-btn-icon-right ui-icon-carat-r">
                        <div style="float: left; margin-top: 2.1rem">
                            <input type="checkbox" style="height:1.5em; width:1.5em; position: absolute;left:1rem;z-index:9000; top:1.2rem" name="chk" value="<% =row["ActivityInstanceId"]%>"/>
                        <%if(!string.IsNullOrEmpty(Botwave.Commons.DbUtils.ToString(row["AliasImage"]))){%>
                          <img alt="<%=row["WorkflowAlias"] %>" class="groupImage" src="<%=AppPath %>contrib/workflow/res/groups/<%=row["AliasImage"] %>" />
                            <%}%>
                        </div>
                        <div style="margin-left: 3rem;" onclick="$('.loading-backdrop').show();$('.loading-backdrop').showLoading();window.location='<%=AppPath%>contrib/mobile/pages/workflowview.aspx?type=review&aiid=<% =row["ActivityInstanceId"]%>&returnurl=<%=HttpUtility.UrlEncode("ToReview.aspx")%>'">
                            <h4 class="list-group-item-heading">
                                <%=row["title"] %></h4>
                            <p class="list-group-item-text">
                                受理号：<%=row["sheetid"] %></p>
                            <p class="list-group-item-text">
                                步&nbsp;&nbsp; 骤：<%=row["ActivityName"] %></p>
                        </div>
                    </div>
    <%}
        if (dtResult.Rows.Count == 0)
        {%>
            <div >
                    <h4 class="list-group-item-heading"><span class="pullDownLabel">无记录显示</span></h4>
                </div>
      <%}%>