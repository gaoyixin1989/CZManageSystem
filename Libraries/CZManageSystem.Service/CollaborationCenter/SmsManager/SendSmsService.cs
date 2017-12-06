using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.SmsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 短信发送表
/// </summary>
namespace CZManageSystem.Service.CollaborationCenter.SmsManager
{
    public class SendSmsService : BaseService<SendSms>, ISendSmsService
    {
        public IList<SendSms> GetForPaging(out int count, SendSmsQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderByDescending(u=>u.Time).Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<SendSms> GetQueryTable(SendSmsQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                SendSmsQueryBuilder obj2 = (SendSmsQueryBuilder)CloneObject(obj);

                if (!string.IsNullOrEmpty(obj.UserName))
                {//发送人
                    curTable = curTable.Where(u => u.SenderObj.RealName.Contains(obj.UserName));
                    obj2.UserName = null;
                }
                if (!string.IsNullOrEmpty(obj.DeptName))
                {//发送部门
                    curTable = curTable.Where(u => u.DeptObj.DpFullName.Contains(obj.DeptName));
                    obj2.DeptName = null;
                }


                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }
    }
}
