using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.DynamicForm.Extension.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    /// <summary>
    /// FormItemIFramesService的接口
    /// </summary>
    public interface IFormItemIFramesService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="formItemIFrames"></param>
        void Create(FormItemIFrames formItemIFrames);
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="formItemIFrames"></param>
        /// <returns></returns>
        int Update(FormItemIFrames formItemIFrames);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="formItemDefinitionId"></param>
        /// <returns></returns>
        int Delete(Guid formItemDefinitionId);

        /// <summary>
        /// 获取指定的APP信息
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        FormItemIFrames LoadById(int appId);

        /// <summary>
        /// 获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        FormItemIFrames LoadByFormItemDefinitionIdAndName(Guid formItemDefinitionId, string activityName);

        /// <summary>
        /// 获取全部已启用的 Apps 信息.
        /// </summary>
        /// <returns></returns>
        IList<FormItemIFrames> SelectByFormItemDefinitionId(Guid formItemDefinitionId);

        /// <summary>
        /// 根据设置类型获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        FormItemIFrames LoadByFormItemDefinitionIdAndType(Guid formItemDefinitionId, int settingType);

        /// <summary>
        /// 根据表单定义ID获取 Apps 信息.
        /// </summary>
        /// <returns></returns>
        IList<FormItemIFrames> SelectByFormDefinitionId(Guid formDefinitionId);

        /// <summary>
        /// 判断指定应用呈现名称是否存在.
        /// </summary>
        /// <param name="formItemDefinitionId"></param>
        /// <param name="activityName"></param>
        /// <param name="settingType"></param>
        /// <returns></returns>
        bool IsExists(Guid formItemDefinitionId, string activityName, int settingType);
    }
}
