using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.IBatisNet
{
    public class AuthorizeService : IAuthorizeService
    {
        #region IAuthorityService 成员

        public void InsertAuthorization(AuthorizationInfo item)
        {
            IBatisMapper.Insert("bw_Authorizations_Insert", item);
        }

        public int UpdateAuthorization(int authorizationId, bool isEnabled)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("Id", authorizationId);
            parameters.Add("Enabled", isEnabled);
            return IBatisMapper.Update("bw_Authorizations_Update_Enabled", parameters);
        }

        public DataTable GetAuthorizationsByPager(Guid fromUserId, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bw_Authorizations_Detail";
            string fieldKey = "Id";
            string fieldShow = "Id, FromUserId, ToUserId, IsFullAuthorized, BeginTime, EndTime, Enabled, FromRealName, ToRealName, ToDpFullName";
            string fieldOrder = "Id DESC";

            string where = string.Format("FromUserId = '{0}'", fromUserId);

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where, ref recordCount);
        }

        #endregion
    }
}
