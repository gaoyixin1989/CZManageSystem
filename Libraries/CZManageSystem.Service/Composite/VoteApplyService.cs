using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Composite
{
    public class VoteApplyService : BaseService<VoteApply>, IVoteApplyService
    {
        ISysUserService sysUserService = new SysUserService();
        /// <summary>
        /// 投票分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime) : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));

            PagedList<VoteApply> pageList = new PagedList<VoteApply>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
            count = pageList.TotalCount;


            //  < th > 操作 </ th >
            return pageList.Select(a => new
            {
                a.ApplyID,
                a.WorkflowInstanceId,
                a.ApplyTitle,    //  主题名称  
                a.ThemeType,    //   主题类型  
                a.Creator,       //   发起人 
                a.StartTime,    //   开始时间 
                a.EndTime,      //   结束时间  
                BodyCount = a.VoteThemeInfo?.VoteJoinPersons.Count.ToString() //答题表中存在的人数
                + "/"
                + (a.MemberType == 1 ? GetCount(a.MemberIDs) : string.IsNullOrEmpty(a.MemberIDs) ? "0" : (a.MemberIDs?.Split(new char[] { ',' })?.Count() - 1)?.ToString()),//参加答题的成员人数
                                                                                                                                                                            // a.TrackingWorkflow?.VoteApplys.Where(w => w.TrackingWorkflow.State == 2).Count().ToString() + "/" + a.TrackingWorkflow?.VoteApplys.Count.ToString(),// 人数统计
                a.VoteThemeInfo.ThemeID,
                //  投票统计
                //  答案明细 
                a.TrackingWorkflow?.State, //   状态  
                a.CreatorID,
                a.MemberName,
                a.MemberIDs,
                CreatedTime = a.CreateTime.ToString()
            });

        }
        /// <summary>
        /// 投票分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForPaging_(out int count, Users user, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = this._entityStore.Table.Where(s => (s.MemberIDs.Contains(user.UserId.ToString()) || s.MemberIDs.Contains(user.DpId))&&s.TrackingWorkflow.State ==2);
            source = objs == null ? source : source.Where(ExpressionFactory(objs));
            PagedList<VoteApply> pageList = new PagedList<VoteApply>(source.OrderByDescending(c => c.StartTime), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
            count = pageList.TotalCount;
            //  < th > 操作 </ th >
            return pageList.Select(a => new
            {
                a.ApplyID,
                a.WorkflowInstanceId,
                a.ApplyTitle,    //  主题名称  
                a.ThemeType,    //   主题类型  
                a.Creator,       //   发起人 
                a.StartTime,    //   开始时间 
                a.EndTime,      //   结束时间  
                //BodyCount = a.TrackingWorkflow?.VoteApplys.Where(w => w.TrackingWorkflow.State == 2).Count().ToString() + "/" + a.TrackingWorkflow?.VoteApplys.Count.ToString(),// 人数统计
                a.VoteThemeInfo.ThemeID,
                //  投票统计  //  答案明细 
                State = CompareDate(a.StartTime, a.EndTime), //   状态  
                a.CreatorID,
                a.MemberName,
                a.MemberIDs,
                CreatedTime = a.CreateTime.ToString()
            });

        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForDetailPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime) : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));

            PagedList<VoteApply> pageList = new PagedList<VoteApply>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            count = pageList.TotalCount;

            //  < th > 操作 </ th >
            return pageList.Select(a => new
            {
                a.ApplyID,
                a.WorkflowInstanceId,
                a.ApplyTitle,    //< th > 主题名称 </ th >
                a.ThemeType,    //  < th > 主题类型 </ th >
                a.Creator,       //  < th > 发起人 </ th >
                                 //a.StartTime,    //  < th > 开始时间 </ th >
                                 //a.EndTime,      //  < th > 结束时间 </ th >
                                 //  < th > 人数统计 </ th >

                //  < th > 投票统计 </ th > 
                //  < th > 答案明细 </ th > 
                //a.Tracking_Todo?.State, //  < th > 状态 </ th >
                a.CreatorID,
                //a.MemberName,
                //a.MemberIDs,
                //CreatedTime = a.CreateTime.ToString()


                Questions = a.VoteThemeInfo?.VoteTidQies?.Select(q => q.VoteQuestion)?.Select(v => new
                {
                    v.AnswerNum,
                    v.Creator,
                    v.CreateTime,
                    v.CreatorID,
                    v.IsDel,
                    v.MaxValue,
                    v.MinValue,
                    v.QuestionID,
                    v.QuestionTitle,
                    v.QuestionType,
                    v.Remark,
                    v.State,
                    VoteAnsers = v.VoteAnsers.Select(c => new
                    {
                        c.AnserContent,
                        c.AnserID,
                        c.AnserScore,
                        c.MaxValue,
                        c.MinValue,
                        c.QuestionID,
                        c.SortOrder//,
                                   // AnsersCount = string.IsNullOrEmpty(type) ? 0 : a.VoteSelectedAnsers.Count
                    }),
                    //QuestionCount = string.IsNullOrEmpty(type) ? 0 : v.VoteSelectedAnsers.Count,
                    VoteSelectedAnsers = v.VoteSelectedAnsers.Select(l => new
                    {
                        l.OtherContent,
                        l.Respondent  //sysUserService
                                      //.List ().Where(u => u.UserId == l.UserID).FirstOrDefault ().RealName 
                                      // .FindByFeldName (u=>u.UserId == l.UserID ).RealName 
                    })
                })
            })
            ;


        }
        bool CompareDate(DateTime? star, DateTime? end)
        {
            var now = DateTime.Now;
            if (now > star && now < end)
                return true;
            return false;
        }
        string GetCount(string ids)
        {
            int count = sysUserService.List().Where(l => ids.Contains(l.DpId)).Count();
            return count.ToString();
        }
    }
}
