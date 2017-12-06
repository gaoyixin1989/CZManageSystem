using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Composite
{
    public class V_VoteDetailService : BaseService<V_VoteDetail>, IV_VoteDetailService
    {

        IVoteSelectedAnserService voteSelectedAnserService = new VoteSelectedAnserService();
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
            var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.CreateTime)
                : this._entityStore.Table.OrderByDescending(c => c.CreateTime).Where(ExpressionFactory(objs));
            var pageList = new PagedList<V_VoteDetail>().QueryPagedList(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            var list = pageList.ToList().Select(s =>
                GetModel(s)
            );
            return list;
        }
        dynamic GetModel(V_VoteDetail v)
        {
            var sa = voteSelectedAnserService.List().Where(f => f.QuestionID == v.QuestionID && f.UserID == v.UserID).ToList();
            return new
            {
                v.AnswerNum,
                v.CreateTime,
                v.QuestionID,
                v.Creator,
                v.CreatorID,
                v.QuestionTitle,
                v.ApplyTitle,
                v.ThemeType,
                v.UserID,
                v.UserName,
                v.RealName,
                OtherContent = sa.FirstOrDefault().OtherContent ,// string.Join("；", sa.Select(w => w.OtherContent)),
                AnserContent = string.Join("；", sa.FindAll(f => f.AnserID != -1).Select(w => w.VoteAnser?.AnserContent))
            };

        }
    }
}
