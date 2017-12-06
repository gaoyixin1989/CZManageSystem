using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Extension.Domain;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程操作人的服务接口.
    /// </summary>
    public interface IWorkflowUserService
    {
        /// <summary>
        /// 获取指定用户名的操作人信息.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        ActorDetail GetActorDetail(string userName);

        /// <summary>
        /// 获取步骤操作人提示信息.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        TooltipInfo GetActorTooltip(string userName);

        /// <summary>
        /// 获取指定匹配用户名或者真实姓名的用户真实姓名列表.
        /// </summary>
        /// <param name="prefixName"></param>
        /// <param name="count">取得的列表大小.</param>
        /// <returns></returns>
        IList<string> GetActorsLikeName(string prefixName, int count);

        /// <summary>
        /// 获取指定操作人用户名集合的用户真实姓名字典.
        /// </summary>
        /// <param name="actorNames"></param>
        /// <returns></returns>
        IDictionary<string, string> GetActorRealNames(ICollection<string> actorNames);
    }
}
