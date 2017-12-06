using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace CZManageSystem.Service.SysManger
{
    public class SysLogService
    {
        MongoDbHelper monHelper = new MongoDbHelper();

        /// <summary>
        /// 获取系统错误日志
        /// </summary>
        /// <param name="total"></param>
        /// <param name="username"></param>
        /// <param name="time_start"></param>
        /// <param name="time_end"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> GetErrorLogList(out Int64 total, string username ,string time_start,string time_end, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            List<IMongoQuery> query = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(username))
                query.Add(Query.EQ("UserName", username));
            if (!string.IsNullOrEmpty(time_start))
                query.Add(Query.GTE("ErrorTime", time_start));
            if (!string.IsNullOrEmpty(time_start))
                query.Add(Query.LTE("ErrorTime", time_end));
            SortByDocument sort = new SortByDocument { { "ErrorTime", -1 } };

            return monHelper.Find<SysErrorLog>(out total, (query.Count > 0 ? Query.And(query) : null), pageIndex, pageSize, sort);

        }
        /// <summary>
        /// 获取系统操作日志
        /// </summary>
        /// <param name="total"></param>
        /// <param name="operationType"></param>
        /// <param name="username"></param>
        /// <param name="time_start"></param>
        /// <param name="time_end"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> GetOperationLogList(out Int64 total, string operationType,string username, string time_start, string time_end, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            List<IMongoQuery> query =new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(operationType))
                query.Add(Query.EQ("Operation", Convert.ToInt32(operationType)));
            if (!string.IsNullOrEmpty(username))
                query.Add(Query.EQ("UserName", username));
            if (!string.IsNullOrEmpty(time_start))
                query.Add(Query.GTE("OperationTime", time_start));
            if (!string.IsNullOrEmpty(time_start))
                query.Add(Query.LTE("OperationTime", time_end));
            
            SortByDocument sort = new SortByDocument { { "OperationTime", -1 } };

            return monHelper.Find<SysOperationLog>(out total, (query.Count>0?Query.And(query):null), pageIndex, pageSize, sort);

        }
        /// <summary>
        /// 获取服务策略日志
        /// </summary>
        /// <param name="total"></param>
        /// <param name="ServiceStrategyId">服务id</param>
        /// <param name="time_start"></param>
        /// <param name="time_end"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> GetServiceStrategyLogList(out Int64 total, int ServiceStrategyId, string time_start, string time_end, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            List<IMongoQuery> query = new List<IMongoQuery>();
                query.Add(Query.EQ("ServiceStrategyId", ServiceStrategyId));
            if (!string.IsNullOrEmpty(time_start))
                query.Add(Query.GTE("LogTime", time_start));
            if (!string.IsNullOrEmpty(time_start))
                query.Add(Query.LTE("LogTime", time_end));
            SortByDocument sort = new SortByDocument { { "LogTime", -1 } };

            return monHelper.Find<SysServiceStrategyLog>(out total, (query.Count > 0 ? Query.And(query) : null), pageIndex, pageSize, sort);

        }
        public Task<bool> WriteSysLog<T>(T log)
        {
            return  Task.Run<bool>(() => { return monHelper.Insert<T>(log); });
        }
        public Task<bool> WriteSysLog<T>(List<T> logList)
        {
            return Task.Run<bool>(() => { return monHelper.Insert<T>(logList); });
        }
    }
}
