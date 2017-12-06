using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.XQP.Domain;

public partial class contrib_workflow_controls_ProcessHistoryExtend : Botwave.XQP.Web.Controls.WorkflowProcessHistory
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void DataBind(string historyHtml)
    {
        this.ltlHistoryLogs.Text = historyHtml;
    }

    // ActivityId, CZActivityInstance
    IDictionary<Guid, IList<CZActivityInstance>> uncompletedActivities = new Dictionary<Guid, IList<CZActivityInstance>>();

    protected override void OnPreBuildProcessHistory(ref bool isOutputUncompleted, ref IList<CZActivityInstance> workflowActivities)
    {
        isOutputUncompleted = true;
        for (int i = 0; i < workflowActivities.Count; i++)
        {
            CZActivityInstance item = workflowActivities[i];
            if (!item.IsCompleted)
            {
                Guid prevSetID = item.PrevSetId;
                if (uncompletedActivities.ContainsKey(prevSetID))
                {
                    uncompletedActivities[prevSetID].Add(item);
                    workflowActivities.RemoveAt(i);
                    i--;
                }
                else
                {
                    uncompletedActivities.Add(prevSetID, new List<CZActivityInstance>());
                    uncompletedActivities[prevSetID].Add(item);
                }
            }
        }
    }

    protected override string BuildActivityNameCell(CZActivityInstance item, int rowCount)
    {
        string rowSpan = (rowCount > 1 ? string.Format(" rowspan=\"{0}\"", rowCount) : string.Empty);  // 流程活动名称单元格的 rowspan 属性.
        string previousActors = BuildPreviouseActors(item);
        return base.BuildActivityNameCell(item, rowCount) + string.Format("<td{0}>{1}</td>", rowSpan, previousActors);
    }

    protected override string BuildTodoActors(CZActivityInstance item)
    {
        if (uncompletedActivities.ContainsKey(item.PrevSetId))
        {
            string result = string.Empty;
            IList<BasicUser> users = GeTodotActors(uncompletedActivities[item.PrevSetId]); 
            if (users != null && users.Count > 0)
            {
                foreach (Botwave.Entities.BasicUser user in users)
                    result += string.Format(",<span tooltip=\"{0}\"><b>{1}</b></span>", user.UserName, user.RealName);
                result = result.Remove(0, 1);
            }
            return result;
        }
        return string.Empty;
    }

    protected virtual string BuildPreviouseActors(CZActivityInstance item)
    {
        string previousActors = item.PreviousActors;
        if (!item.IsCompleted)
        {
            if (uncompletedActivities.ContainsKey(item.PrevSetId))
            {
                IList<CZActivityInstance> activities = uncompletedActivities[item.PrevSetId];
                if (activities.Count > 1)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (CZActivityInstance activityInstance in activities)
                    {
                        builder.AppendFormat("{0},", activityInstance.PreviousActors);
                    }
                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                        string[] names = builder.ToString().Split(',', '，');
                        if (names != null && names.Length > 0)
                        {
                            IDictionary<string, string> dict = new Dictionary<string, string>(StringComparer.CurrentCulture);
                            foreach (string name in names)
                            {
                                string key = name.Trim();
                                if (string.IsNullOrEmpty(key) || dict.ContainsKey(key))
                                    continue;
                                dict.Add(key, key);
                            }
                            builder = new StringBuilder();
                            foreach (string key in dict.Keys)
                                builder.AppendFormat("{0},", key);
                            if (builder.Length > 0)
                            {
                                builder.Length =  builder.Length - 1;
                                previousActors = builder.ToString();
                            }
                        }
                    }
                }
            }
        }
        return WorkflowUtility.FormatWorkflowActor(previousActors, string.Empty);
    }

    private static IList<BasicUser> GeTodotActors(IList<CZActivityInstance> activityInstances)
    {
        if (activityInstances.Count == 0)
            return new List<BasicUser>();

        IList<BasicUser> results = new List<BasicUser>();
        string sql = @"SELECT DISTINCT  td.UserName, u.RealName FROM bwwf_Tracking_Todo AS td 
                                  LEFT JOIN bw_Users AS u ON u.UserName = td.UserName WHERE (td.ActivityInstanceId IN ({0}))";
        string where = string.Empty;
        foreach (CZActivityInstance item in activityInstances)
        {
            where += string.Format(",'{0}'", item.ActivityInstanceId);
        }
        where = where.Remove(0, 1);
        sql = string.Format(sql, where);
        DataTable table = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        foreach (DataRow row in table.Rows)
        {
            BasicUser item = new BasicUser(DbUtils.ToString(row["UserName"]), DbUtils.ToString(row["RealName"]));
            results.Add(item);
        }

        return results;
    }
}
