using CZManageSystem.Data;
using CZManageSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    /// <summary>
    /// 分页查询日志信息
    /// </summary>
    public interface IOperateLogService : IBaseService<TB_Log_OperateLog>
    {
        PageList<IList<Log>> GetUserIdByroleId(int pageIndex, int pageSize,string portalID, DateTime? Createdtime_Start, DateTime? Createdtime_End);
        /// <summary>
        /// 记录数据库日子
        /// </summary>
        /// <param name="userName">操作用户</param>
        /// <param name="operation">操作功能</param>
        /// <param name="description">异常操作描述</param>
        /// <param name="exception">异常操作类型</param>
        /// <returns></returns>
        KeyValuePair<bool, string> LoggerHelper(string userName, string operation, string description, string exception);
    }
}
    