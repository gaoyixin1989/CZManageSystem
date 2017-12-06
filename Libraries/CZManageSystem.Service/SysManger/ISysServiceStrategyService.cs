using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    public interface ISysServiceStrategyService : IBaseService<SysServiceStrategy>
    {
        /// <summary>
        /// 根据服务名称（表SysServices的ServiceName）查询数据
        /// </summary>
        /// <param name="count">数据总数</param>
        /// <param name="pageIndex">页码,从0开始</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="ServiceName">服务名称</param>
        /// <returns></returns>
        IList<SysServiceStrategy> QueryDataByServiceName(out int count, int pageIndex = 0, int pageSize = int.MaxValue, string ServiceName = null);

        /// <summary>
        /// 获取有效的服务策略信息
        /// </summary>
        /// <returns></returns>
        IList<SysServiceStrategy> GetValidServiceStrategyData();

    }
}