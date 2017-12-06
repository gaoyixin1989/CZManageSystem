using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.SysManger
{
    public class OperateLogService : BaseService<TB_Log_OperateLog>, IOperateLogService
    {
        private readonly IRepository<TB_Log_OperateLog> _log_OperateLog;
        private readonly IRepository<TB_Log_ExceptionCatalog> _log_ExceptionCatalog;
        private readonly IRepository<TB_Log_OperationCatalog> _log_OperationCatalog;

        public OperateLogService()
        {
            this._log_OperateLog = new EfRepository<TB_Log_OperateLog>();
            this._log_ExceptionCatalog = new EfRepository<TB_Log_ExceptionCatalog>();
            this._log_OperationCatalog = new EfRepository<TB_Log_OperationCatalog>();
        }
        string clientIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        string clientComputerName = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        string serverIP = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        /// <summary>
        /// 记录数据库日子
        /// </summary>
        /// <param name="userName">操作用户</param>
        /// <param name="operation">操作功能</param>
        /// <param name="description">异常操作描述</param>
        /// <param name="exception">异常操作类型</param>
        /// <returns></returns>
        public KeyValuePair<bool, string> LoggerHelper(string userName, string operation, string description, string exception)
        {
            var operationLog = this._log_OperationCatalog.Table.FirstOrDefault(x => x.operationName == operation);
            var operationId = default(string);
            if (operationLog == null)
                return new KeyValuePair<bool, string>(false, "operation未找到");
            else
                operationId = operationLog.operationID;
            var exceptionLog = this._log_ExceptionCatalog.Table.FirstOrDefault(x => x.exceptionName == exception);
            var exceptionId = default(string);
            if (exceptionLog == null)
                return new KeyValuePair<bool, string>(false, "exception未找到");
            else
                exceptionId = exceptionLog.exceptionID;

            TB_Log_OperateLog operateLog = new TB_Log_OperateLog()
            {
                opEndTime = DateTime.Now,
                opStartTime = DateTime.Now,
                portalID = userName,
                clientIP = clientIP,
                clientComputerName = clientComputerName,
                serverIP = serverIP,
                operationID = operationId,
                exceptionID = exceptionId,
                description = description

            };
            this._entityStore.InsertAsync(operateLog);
            return new KeyValuePair<bool, string>(true, "成功");


        }
    public PageList<IList<Log>> GetUserIdByroleId(int pageIndex, int pageSize,string portalID, DateTime? Createdtime_Start, DateTime? Createdtime_End)
        {
            var query = from lo in this._log_OperateLog.Table
                        join le in this._log_ExceptionCatalog.Table
                        on lo.exceptionID equals le.exceptionID
                        join lc in this._log_OperationCatalog.Table 
                        on lo.operationID equals lc.operationID
                        select new
                        {
                            uid = lo.uid,
                            opStartTime = lo.opStartTime,
                            portalID = lo.portalID,
                            description = lo.description,
                            exceptionName = le.exceptionName,
                            operationName = lc.operationName

                        };
            if (!string.IsNullOrEmpty(portalID))
                query = query.Where(x => x.portalID == portalID);
            if (Createdtime_Start != null && Createdtime_End != null)
                query = query.Where(x => x.opStartTime >= Createdtime_Start && x.opStartTime <= Createdtime_End);

            var pageQuery = query.OrderByDescending(c => c.opStartTime)
                         .Skip(pageSize * pageIndex <= 0 ? 0 : (pageIndex - 1)).Take(pageSize)
                         .AsEnumerable().Select(x => new Log()
                         {
                             opStartTime = x.opStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                             description = x.description,
                             exceptionName = x.exceptionName,
                             operationName = x.operationName,
                             portalID = x.portalID,
                             uid = x.uid
                         });

            var count = query.Count();
            return new PageList<IList<Log>>(count, pageQuery.ToList());
        }
    }
}
