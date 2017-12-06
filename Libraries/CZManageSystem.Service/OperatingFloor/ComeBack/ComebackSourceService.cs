using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.OperatingFloor.ComeBack
{
    public class ComebackSourceService : BaseService<ComebackSource>, IComebackSourceService
    {

        // 报表数据  没有使用
        public IEnumerable<dynamic> GetReport(out int count, ComebackReporteQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            count = 0;
            var curTable = this._entityStore.Table;
            if (objs.YearStart != null)
                curTable = curTable.Where(a => a.Year >= objs.YearStart);
            if (objs.YearEnd != null)
            {
                curTable = curTable.Where(a => a.Year <= objs.YearEnd);
            }
            if (objs.BudgetDept != null)
                curTable = curTable.Where(a => a.BudgetDept.Contains(objs.BudgetDept));
            return new PagedList<ComebackSource>(curTable.OrderByDescending(c => new { c.Year, c.BudgetDept }), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count).Select(s => new
            {
                SourceBudgetDept= s.BudgetDept,
                SourceYear=s.Year,
                SourceName= s.Name,
                SourceAmount=s.Amount,
                SourceRemainAmount = s.Amount == null ? 0 : s.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[getComebackSourceAmount]('" + s.ID + "')").First(),
                ComebackTypes = s.ComebackTypes.Select(c => new
                { 
                    TypeBudgetDept = c.BudgetDept,
                    TypeAmount = c.Amount,
                    TypeRemainAmount = c.Amount == null ? 0 : c.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[getComebackTypeAmount]('" + c.ID + "')").First(),
                    Childs = c.ComebackChilds.Select(f => new
                    {
                        ////ChildID=f.ID,
                        ////ChildName=f.Name,
                        ////ChildPID = f.PID,
                        ////ChildYear= f.Year,
                        ChildAmount = f.Amount,
                        ChildRemainAmount = f.Amount == null ? 0 : f.Amount - this._entityStore.Execute<decimal>(" select  [dbo].[GetComebackChildAmount]('" + f.ID + "')").First()
                    })
                })
            });
        }

    }
}
