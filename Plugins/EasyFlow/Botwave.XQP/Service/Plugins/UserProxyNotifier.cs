using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.GMCCServiceHelpers;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Allocator;

namespace Botwave.XQP.Service.Plugins
{
    public class UserProxyNotifier : IUserProxyNotifier
    {
        private IUserService userService;

        public IUserService UserService
        {
            set { userService = value; }
        }

        #region IUserProxyNotifier Members

        public void Notify(Guid activityInstanceId, IDictionary<string, string> relations)
        {
            IDictionary<string, string> users = FilterUsers(relations);
            if (users.Count > 0)
            {
                string title = null;
                string creator = null;
                string creatorName = null;
                string entityId = activityInstanceId.ToString();
                string sql = String.Format(@"select w.Title, u.RealName, w.Creator
from bwwf_Tracking_Activities as ta
	left join bwwf_Tracking_Workflows as w on ta.WorkflowInstanceId = w.WorkflowInstanceId
	left join bw_Users as u on w.Creator = u.UserName
where ta.ActivityInstanceId = '{0}'", entityId);
                using (IDataReader reader = IBatisDbHelper.ExecuteReader(CommandType.Text, sql))
                {
                    if (reader.Read())
                    {
                        title = reader.GetString(0);
                        creatorName = reader.GetString(1);
                        creator = reader.GetString(2);
                    }
                    reader.Close();
                }

                if (null != title)
                {
                    foreach (string key in users.Keys)
                    {
                        UserInfo user = userService.GetUserByUserName(key);

                        if (null != user)
                        {
                            string pendingTitle = String.Format("{0}({1}发起,{2}代审)", title, creatorName, user.RealName);
                            string url = WorkflowPostHelper.TransformViewUrlByActivityInstanceId(entityId);
                            AsynExtendedPendingJobHelper.AddPendingMsg(users[key], creator, pendingTitle, url, ActivityInstance.EntityType, entityId);
                        }
                    }
                }
            }
        }

        #endregion

        static IDictionary<string, string> FilterUsers(IDictionary<string, string> relations)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> entry in relations)
            {
                if (!String.IsNullOrEmpty(entry.Value))
                {
                    result[entry.Key] = entry.Value;
                }
            }
            return result;
        }
    }
}
