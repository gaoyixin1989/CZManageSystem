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
    public class VoteQuestionService : BaseService<VoteQuestion>, IVoteQuestionService
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
            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime) : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));

            PagedList<VoteQuestion> pageList = new PagedList<VoteQuestion>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
            count = pageList.TotalCount;

            //  < th > 操作 </ th >
            return pageList.Select(a => new
            {
                a.AnswerNum,
                a.CreateTime,
                a.QuestionID,
                a.Creator,
                a.CreatorID,
                a.QuestionTitle,
                a.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ApplyTitle,
                a.VoteTidQies.FirstOrDefault()?.VoteThemeInfo?.ThemeName,
                a.VoteTidQies.FirstOrDefault()?.VoteThemeInfo.VoteApplies.FirstOrDefault()?.ThemeType,
                VoteSelectedAnsers = a.VoteSelectedAnsers.Select(
                    v => new
                    {
                        v.OtherContent,
                        v.UserID,
                        v.Respondent,
                        v.AnserID,
                        v.VoteAnser.AnserContent

                    }
                    )
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
        public IEnumerable<dynamic> GetForPaging_(out int count,Guid CreatorID, string ApplyTitle = null, string ThemeID = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                IQueryable<VoteQuestion> source;
                int _ThemeID = 0;
                    int.TryParse(ThemeID,out _ThemeID);
                if (_ThemeID > 0 ) 
                    source = this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(r => r.VoteTidQies.FirstOrDefault().ThemeID== _ThemeID); 
                else 
                    source = this._entityStore.Table.OrderByDescending(c => c.CreateTime);
                if (ApplyTitle != null)
                    source= source.Where(s => s.VoteTidQies.FirstOrDefault().VoteThemeInfo.VoteApplies.FirstOrDefault().ApplyTitle.Contains(ApplyTitle));
                     
                PagedList<VoteQuestion> pageList = new PagedList<VoteQuestion>(source.Where(w => w.VoteSelectedAnsers.Count > 0&&w.CreatorID== CreatorID), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count, false);
                count = pageList.TotalCount;

                return pageList.Select(a => new
                {
                    a.AnswerNum,
                    a.CreateTime,
                    a.QuestionID,
                    a.Creator,
                    a.CreatorID,
                    a.QuestionTitle,
                    a.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ApplyTitle,
                    a.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.ThemeName,
                    a.VoteTidQies?.FirstOrDefault()?.VoteThemeInfo?.VoteApplies?.FirstOrDefault()?.ThemeType,
                    a.VoteSelectedAnsers.FirstOrDefault()?.OtherContent,
                    a.VoteSelectedAnsers.FirstOrDefault()?.UserID,
                    a.VoteSelectedAnsers.FirstOrDefault()?.Respondent,
                    AnserContent = string.Join("；", a.VoteSelectedAnsers.Where(s => s.AnserID != -1).Select(w => w.VoteAnser?.AnserContent))
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
