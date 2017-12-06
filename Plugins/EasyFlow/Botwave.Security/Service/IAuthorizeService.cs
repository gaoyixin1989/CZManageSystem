using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Security.Domain;

namespace Botwave.Security.Service
{
    /// <summary>
    /// 用户授权服务接口.
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 新增授权信息.
        /// </summary>
        /// <param name="item"></param>
        void InsertAuthorization(AuthorizationInfo item);

        /// <summary>
        /// 启用或者停止授权.
        /// </summary>
        /// <param name="authorizationId"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        int UpdateAuthorization(int authorizationId, bool isEnabled);

        /// <summary>
        /// 分页获取指定授权人编号的授权记录.
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetAuthorizationsByPager(Guid fromUserId, int pageIndex, int pageSize, ref int recordCount);
    }
}
