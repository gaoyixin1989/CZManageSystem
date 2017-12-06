using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative
{
    public class CarApplyService : BaseService<CarApply>, ICarApplyService
    {
        ITracking_TodoService tracking_TodoService = new Tracking_TodoService();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        static List<DataDictionary> dataDictionary;
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.ApplyId) : this._entityStore.Table.OrderBy(c => c.ApplyId).Where(ExpressionFactory(objs));
            PagedList<CarApply> pageList = new PagedList<CarApply>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
            count = pageList.TotalCount;
            #region Select
            return pageList.Select(u => new
            {
                u.Allocator,
                u.ApplyTitle,
                u.AllotIntro,
                AllotTime = Convert.ToDateTime(u.AllotTime).ToString("yyyy-MM-dd HH:mm:ss"),
                u.ApplyCant,
                u.ApplyId,
                u.Attach,
                u.BalCount,
                u.BalRemark,
                u.BalTime,
                u.BalTotal,
                u.BalUser,
                u.Boral,
                u.CarIds,
                CorpId_text = GetCorpName(u.CorpId),
                u.CorpId,
                CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd"),
                u.DeptName,
                u.Destination1,
                u.Destination2,
                u.Destination3,
                u.Destination4,
                u.Destination5,
                u.Driver,
                u.EndTime,
                u.Field00,
                u.Field01,
                u.Field02,
                u.FinishTime,
                u.KmCount,
                u.KmNum1,
                u.KmNum2,
                u.Leader,
                u.Mobile,
                u.OpinGrade1,
                u.OpinGrade2,
                u.OpinGrade3,
                u.OpinGrade4,
                u.OpinGrade5,
                u.OpinGrade6,
                u.OpinGrade7,
                u.OpinRemark,
                u.OpinTime,
                u.OpinUser,
                u.PersonCount,
                u.Remark,
                u.Request,
                u.Road,
                u.SpecialReason,
                u.Starting,
                u.StartTime,
                u.TimeOut,
                u.UptTime,
                u.UptUser,
                u.UseType,
                u.WorkflowInstanceId,
                u.TrackingWorkflow?.State,
                ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId?.ToString()) ? u.ApplyCant : GetActorName(u.WorkflowInstanceId)
            });
            #endregion
        }



        public  IEnumerable<dynamic> GetCarEvaluation(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        { 
            var source = objs == null ? this._entityStore.Table.OrderBy(c => c.ApplyId).Where (w=>w.TrackingWorkflow.State ==2) : this._entityStore.Table.OrderBy(c => c.ApplyId).Where(w => w.TrackingWorkflow.State == 2).Where(ExpressionFactory(objs));
            PagedList<CarApply> pageList = new PagedList<CarApply>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, true);
            count = pageList.TotalCount;
            #region Select
            return pageList.Select(u => new
            {
                u.Allocator,
                u.ApplyTitle,
                u.AllotIntro,
                AllotTime = Convert.ToDateTime(u.AllotTime).ToString("yyyy-MM-dd HH:mm:ss"),
                u.ApplyCant,
                u.ApplyId,
                u.Attach,
                u.BalCount,
                u.BalRemark,
                u.BalTime,
                u.BalTotal,
                u.BalUser,
                u.Boral,
                u.CarIds,
                CorpId_text = GetCorpName(u.CorpId),
                u.CorpId,
                CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd"),
                u.DeptName,
                u.Destination1,
                u.Destination2,
                u.Destination3,
                u.Destination4,
                u.Destination5,
                u.Driver,
                EndTime = Convert.ToDateTime(u.EndTime).ToString("yyyy-MM-dd HH:mm:ss"),
                u.Field00,
                u.Field01,
                u.Field02,
                u.FinishTime,
                u.KmCount,
                u.KmNum1,
                u.KmNum2,
                u.Leader,
                u.Mobile,
                u.OpinGrade1,
                u.OpinGrade2,
                u.OpinGrade3,
                u.OpinGrade4,
                u.OpinGrade5,
                u.OpinGrade6,
                u.OpinGrade7,
                u.OpinRemark,
                u.OpinTime,
                u.OpinUser,
                u.PersonCount,
                u.Remark,
                u.Request,
                u.Road,
                u.SpecialReason,
                u.Starting,
                StartTime = Convert.ToDateTime(u.StartTime).ToString("yyyy-MM-dd HH:mm:ss"),
                TimeOut = Convert.ToDateTime(u.TimeOut).ToString("yyyy-MM-dd HH:mm:ss"),
                u.UptTime,
                u.UptUser,
                u.UseType,
                u.WorkflowInstanceId,
                u.TrackingWorkflow?.State,
                ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId?.ToString()) ? u.ApplyCant : GetActorName(u.WorkflowInstanceId)
            });
            #endregion
        }

        string GetActorName(Guid? workflowInstanceId)
        {
            var list = tracking_TodoService.List().Where(w => w.WorkflowInstanceId == workflowInstanceId).Select(s => s.ActorName);
            return string.Join(",", list);
        }
        string GetCorpName(int? corpId)
        {
            if (corpId == null)
                return "";
            if (dataDictionary == null)
                dataDictionary = _dataDictionaryService.List().Where(f => f.DDName == "所属单位").ToList();
            return dataDictionary.Find(f => f.DDValue == corpId.ToString()).DDText;
        }
    }
}
