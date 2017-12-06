using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Composite
{
    public class VoteSelectedAnserService : BaseService<VoteSelectedAnser>, IVoteSelectedAnserService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.SelectedAnserID) : this._entityStore.Table.OrderByDescending(c => c.SelectedAnserID).Where(ExpressionFactory(objs));

                PagedList<VoteSelectedAnser> pageList = new PagedList<VoteSelectedAnser>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
                count = pageList.TotalCount;
                 
                //  < th > 操作 </ th >
                return pageList .Select(a => new
                {
                    a.AnserID,
                    a.OtherContent,
                    a.QuestionID,
                    a.Respondent,
                    a.SelectedAnserID,
                    a.ThemeID,
                    a.UserID,
                    a.VoteQuestion?.QuestionTitle,
                    a.VoteAnser?.AnserContent,
                    a.VoteQuestion?.Creator ,
                    a.VoteQuestion?.CreatorID,
                    a.VoteQuestion?.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ApplyTitle,
                    a.VoteQuestion?.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ThemeType,
                }).GroupBy(g =>new { g.QuestionID, g.UserID }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public  IEnumerable<dynamic> GetForPaging_(out int count, string ApplyTitle = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = ApplyTitle == null ? this._entityStore.Table.OrderByDescending(c => c.SelectedAnserID) : this._entityStore.Table.OrderByDescending(c => c.SelectedAnserID).Where(s=> s.VoteQuestion.VoteTidQies.FirstOrDefault().VoteThemeInfo.VoteApplies.FirstOrDefault().ApplyTitle.Contains(ApplyTitle));

                PagedList<VoteSelectedAnser> pageList = new PagedList<VoteSelectedAnser>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
                count = pageList.TotalCount;

                //  < th > 操作 </ th >
                return pageList.Select(a => new
                {
                    a.AnserID,
                    a.OtherContent,
                    a.QuestionID,
                    a.Respondent,
                    a.SelectedAnserID,
                    a.ThemeID,
                    a.UserID,
                    a.VoteQuestion?.QuestionTitle,
                    a.VoteAnser?.AnserContent,
                    a.VoteQuestion?.Creator,
                    a.VoteQuestion?.CreatorID,
                    a.VoteQuestion?.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ApplyTitle,
                    a.VoteQuestion?.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ThemeType,
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
