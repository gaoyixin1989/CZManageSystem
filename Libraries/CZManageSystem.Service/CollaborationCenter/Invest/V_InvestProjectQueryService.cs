using CZManageSystem.Data;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.CollaborationCenter.Invest
{
    public class V_InvestProjectQueryService : BaseService<V_InvestProjectQuery>, IV_InvestProjectQueryService
    {
        public IList<V_InvestProjectQuery> GetForPaging(out int count, VInvestProjectQueryQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<V_InvestProjectQuery> GetQueryTable(VInvestProjectQueryQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                VInvestProjectQueryQueryBuilder obj2 = (VInvestProjectQueryQueryBuilder)CloneObject(obj);
                if (!string.IsNullOrEmpty(obj.DpCode_Text))
                {//负责专业室
                    var DpCodeList = new Service.SysManger.SysDeptmentService().List().Where(u => u.DpName.Contains(obj.DpCode_Text)).Select(u => u.DpId).ToList();
                    curTable = curTable.Where(u => DpCodeList.Contains(u.DpCode));
                    obj2.DpCode_Text = string.Empty;
                }
                if (!string.IsNullOrEmpty(obj.UserID_Text))
                {//室负责人
                    var UserIDList = new Service.SysManger.SysUserService().List().Where(u => u.RealName.Contains(obj.UserID_Text)).Select(u => u.UserId).ToList();
                    curTable = curTable.Where(u => UserIDList.Contains(u.UserID.Value));
                    obj2.UserID_Text = string.Empty;
                }
                if (!string.IsNullOrEmpty(obj.ManagerID_Text))
                {//项目经理
                    var UserIDList = new Service.SysManger.SysUserService().List().Where(u => u.RealName.Contains(obj.ManagerID_Text)).Select(u => u.UserId).ToList();
                    curTable = curTable.Where(u => UserIDList.Contains(u.ManagerID.Value));
                    obj2.ManagerID_Text = string.Empty;
                }

                if (!string.IsNullOrEmpty(obj.ProjectName))
                {//项目名称
                    var projectIdList = new InvestProjectService().List().Where(u => u.ProjectName.Contains(obj.ProjectName)).Select(u => u.ProjectID).ToList();
                    curTable = curTable.Where(u => projectIdList.Contains(u.ProjectID));
                    obj2.ProjectName = null;
                }

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }
    }
}
