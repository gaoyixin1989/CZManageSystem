using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.ITSupport
{
    public partial class EquipApplyService : BaseService<EquipApp>, IEquipApplyService
    {
        private readonly IRepository<EquipApp> _EquipApp;
        private readonly IRepository<StockAsset> _StockAsset;
        private readonly IRepository<Tracking_Todo> _Tracking_Todo;
        private readonly IRepository<Stock> _Stock;
       

        public EquipApplyService()
        {
            this._Stock = new EfRepository<Stock>();
            this._StockAsset = new EfRepository<StockAsset>();
            this._EquipApp = new EfRepository<EquipApp>();
            this._Tracking_Todo = new EfRepository<Tracking_Todo>();
        }
        public IEnumerable<dynamic> GetApplyList(out int count, string ApplyTitle = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            count = 0;
            var curTable = this._entityStore.Table;
            if (ApplyTitle != null)
                curTable = curTable.Where(a => a.ApplyTitle == ApplyTitle);
            return new PagedList<EquipApp>(curTable.OrderByDescending(c => c.EditTime), pageIndex <= 0 ? 0 : pageIndex, pageSize, out count).Select(a => new             
            {
                a.ApplyId,
                a.ApplyName,
                a.ApplyTime,
                a.ApplyTitle,
                a.WorkflowInstanceId,
                a.EditTime,
                a.Status,
                ActorName = string.IsNullOrEmpty(a.WorkflowInstanceId?.ToString()) ? a.ApplyName : GetActorName(a.WorkflowInstanceId)
            });
        }
        /// <summary>
        /// 获取流程状态名称说明
        /// </summary>
        public string GetFlowStateText(int state)
        {
            string strResult = "未提交";
            if (!string.IsNullOrEmpty(state.ToString()))
            {
                switch (state)
                {
                    case 1: strResult = "已提交"; break;
                    case 2: strResult = "已完成"; break;
                    case 99: strResult = "已取消"; break;
                    default: break;
                }
            }
            return strResult;
        }
        string GetActorName(Guid? workflowInstanceId)
        {
            var list =new Tracking_TodoService().List().Where(w => w.WorkflowInstanceId == workflowInstanceId).Select(s => s.ActorName);
            return string.Join(",", list);
        }

        public IEnumerable<dynamic> GetAssetSn(string EquipClass)
        {
            if (EquipClass != "")
            {
                var obj = from s in this._Stock.Table
                          where s.EquipClass == EquipClass join a in this._StockAsset.Table on
                          s.Id equals a.StockId
                          select new {
                              s.EquipInfo,
                              s.ProjSn,
                              s.StockType,
                              a.AssetSn
                          };
                return obj.ToList();
            }
            return null;

        }
        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="WorkflowInstanceId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetApplyDetail(Guid WorkflowInstanceId)
        {
            var query = from a in this._EquipApp.Table where a.WorkflowInstanceId== WorkflowInstanceId
                        join t in this._Tracking_Todo.Table on a.WorkflowInstanceId equals t.WorkflowInstanceId
                        select new
                        {
                            a.ApplyId,
                            a.ApplyName,
                            a.ApplyTime,
                            a.ApplyTitle,
                            a.WorkflowInstanceId,
                            a.EditTime,
                            a.ApplyReason,
                            a.ApplySn,
                            a.AppNum,
                            a.BUsername,
                            a.Deptname,
                            a.EquipClass,
                            a.EquipInfo,
                            a.CancleReason,a.AssetSn,
                            t.ActivityName,
                            t.ActorName,
                            t.State
                        };
            query.Select(a => new {
                a.ApplyId,
                a.ApplyName,
                a.ApplyTime,
                a.ApplyTitle,
                a.WorkflowInstanceId,
                a.EditTime,
                a.ApplyReason,
                a.ApplySn,
                a.AppNum,
                a.BUsername,
                a.Deptname,
                a.EquipClass,
                a.EquipInfo,
                a.CancleReason,
                a.AssetSn,
                a.ActivityName,
                a.ActorName,
                a.State,
                StateText = GetFlowStateText(a.State)
            });
            return query.ToList();


        }
    }
}
