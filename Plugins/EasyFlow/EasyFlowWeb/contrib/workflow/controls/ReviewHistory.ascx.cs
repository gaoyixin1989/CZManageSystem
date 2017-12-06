using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.XQP.Domain;

public partial class contrib_workflow_controls_ReviewHistory : Botwave.XQP.Web.Controls.WorkflowControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public virtual void Initialize(string workflowName, Guid workflowInstanceId)
    {
        base.Initialize(workflowName);
        if (!this.Visible)
            return;
        this.LoadData(workflowInstanceId);
    }

    public virtual void Initialize(Guid workflowId, Guid workflowInstanceId)
    {
        WorkflowProfile wprofile = WorkflowProfile.LoadByWorkflowId(workflowId);
        if (wprofile == null || !wprofile.IsReview)
        {
            this.Visible = false;
            return;
        }
        this.Initialize(workflowInstanceId);
    }

    public virtual void Initialize(Guid workflowInstanceId)
    {
        this.LoadData(workflowInstanceId);
    }

    private void LoadData(Guid workflowInstanceId)
    {
        DataTable reviewTable = ToReview.GetReviewTable(workflowInstanceId);
        if (reviewTable == null || reviewTable.Rows.Count == 0)
            return;

        DataRow[] readedRows = reviewTable.Select("State = 1", "ReviewTime ASC, UserName ASC");      // 已阅读.
        DataRow[] unreadedRows = reviewTable.Select("State = 0", "CreatedTime ASC, UserName ASC");   // 未阅读.

        int rowIndex = 0;
        StringBuilder builder = new StringBuilder();
        // 加载已阅数据.
        builder.AppendLine(BuildReadedHtml(readedRows, rowIndex));

        // 加载未阅数据.
        builder.AppendLine(BuildUnReadedHtml(unreadedRows, rowIndex));

        this.ltlHistoryLogs.Text = builder.ToString();
    }

    private string BuildReadedHtml(DataRow[] rows, int rowIndex)
    {
        if (rows == null || rows.Length == 0)
            return string.Empty;
        StringBuilder builder = new StringBuilder();
        foreach (DataRow row in rows)
        {
            string activityName = DbUtils.ToString(row["ActivityName"]);
            string senderName = DbUtils.ToString(row["SenderName"]);
            string realName = DbUtils.ToString(row["RealName"]);
            DateTime reviewTime = DbUtils.ToDateTime(row["ReviewTime"]).Value;
            builder.AppendLine((rowIndex % 2 == 0) ? "<tr>" : "<tr class=\"trClass\">");
            builder.AppendFormat("<td>{0}</td><td>{1}</td><td  colspan=\"2\">{2}</td><td>{3:yyyy-MM-dd HH:mm:ss}</td><td></td>", activityName, senderName, realName, reviewTime);
            builder.AppendLine("</tr>");
            rowIndex++;
        }
        return builder.ToString();
    }

    private string BuildUnReadedHtml(DataRow[] rows, int rowIndex)
    {
        if (rows == null || rows.Length == 0)
            return string.Empty;

        IDictionary<Guid, ReviewDataItem> unreadedActivities = new Dictionary<Guid, ReviewDataItem>();
        foreach (DataRow row in rows)
        {
            Guid activityInstanceId = DbUtils.ToGuid(row["ActivityInstanceId"]).Value;
            string activityName = DbUtils.ToString(row["ActivityName"]);
            string senderName = DbUtils.ToString(row["SenderName"]);
            string realName = DbUtils.ToString(row["RealName"]);
            if (string.IsNullOrEmpty(realName))
                continue;
            if (!unreadedActivities.ContainsKey(activityInstanceId))
            {
                ReviewDataItem dataItem = new ReviewDataItem(activityName, senderName);
                unreadedActivities.Add(activityInstanceId, dataItem);
            }
            unreadedActivities[activityInstanceId].Receivers.Add(realName);
        }

        StringBuilder builder = new StringBuilder();
        foreach (Guid key in unreadedActivities.Keys)
        {
            ReviewDataItem dataItem = unreadedActivities[key];
            if (string.IsNullOrEmpty(dataItem.ReceiverNames))
                continue;
            builder.AppendLine((rowIndex % 2 == 0) ? "<tr>" : "<tr class=\"trClass\">");
            builder.AppendFormat("<td>{0}</td><td>{1}</td><td  colspan=\"2\">{2}</td><td></td><td></td>", dataItem.ActivityName, dataItem.Sender, dataItem.ReceiverNames);
            builder.AppendLine("</tr>");
            rowIndex++;
        }
        return builder.ToString();
    }

    [Serializable]
    private class ReviewDataItem
    {
        private string _activityName;
        private string _sender;
        private IList<string> _receivers;

        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public IList<string> Receivers
        {
            get { return _receivers; }
            set { _receivers = value; }
        }

        public string ReceiverNames
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (string item in this.Receivers)
                {
                    builder.AppendFormat("{0},", item);
                }
                if (builder.Length > 0)
                    builder.Length = builder.Length - 1;
                return builder.ToString();
            }
        }

        public ReviewDataItem()
        {
            this._receivers = new List<string>();
        }

        public ReviewDataItem(string activityName, string sender)
            : this()
        {
            this._activityName = activityName;
            this._sender = sender;
        }
    }
}
