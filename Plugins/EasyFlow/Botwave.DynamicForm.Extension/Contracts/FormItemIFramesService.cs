using System;
using System.Collections.Generic;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.DynamicForm.Extension.Implements;
using System.Collections;

namespace Botwave.DynamicForm.Extension.Contracts
{
    /// <summary>
    /// 系统对接(IFrame) 的业务类.
    /// </summary>
    public class FormItemIFramesService : IFormItemIFramesService
    {
        #region 数据操作

        /// <summary>
        /// 创建.
        /// </summary>
        /// <returns></returns>
        public void Create(FormItemIFrames formItemIFrames)
        {
            IBatisMapper.Insert("bwdf_FormItemIFrames_Insert", formItemIFrames);
        }

        /// <summary>
        /// 更新.
        /// </summary>
        /// <returns></returns>
        public int Update(FormItemIFrames formItemIFrames)
        {
            return IBatisMapper.Update("bwdf_FormItemIFrames_Update", formItemIFrames);
        }

        /// <summary>
        /// 删除指定 Apps.
        /// </summary>
        /// <returns></returns>
        public int Delete(Guid formItemDefinitionId)
        {
            return IBatisMapper.Update("bwdf_FormItemIFrames_Delete", formItemDefinitionId);
        }

        /// <summary>
        /// 获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public FormItemIFrames LoadById(int appId)
        {
            return IBatisMapper.Load<FormItemIFrames>("bwdf_FormItemIFrames_Select", appId);
        }

        /// <summary>
        /// 获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public FormItemIFrames LoadByFormItemDefinitionIdAndName(Guid formItemDefinitionId,string activityName)
        {
            FormItemIFrames item = new FormItemIFrames();
            item.FormItemDefinitionId = formItemDefinitionId;
            item.ActivityName = activityName;
            return IBatisMapper.Load<FormItemIFrames>("bwdf_FormItemIFrames_Select_By_Name_And_Id", item);
        }

        /// <summary>
        /// 根据设置类型获取指定的 Apps 信息.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public FormItemIFrames LoadByFormItemDefinitionIdAndType(Guid formItemDefinitionId, int settingType)
        {
            FormItemIFrames item = new FormItemIFrames();
            item.FormItemDefinitionId = formItemDefinitionId;
            item.SettingType = settingType;
            return IBatisMapper.Load<FormItemIFrames>("bwdf_FormItemIFrames_Select_By_Type_And_Id", item);
        }

        /// <summary>
        /// 获取全部 Apps 信息.
        /// </summary>
        /// <returns></returns>
        public IList<FormItemIFrames> SelectByFormItemDefinitionId(Guid formItemDefinitionId)
        {
            return IBatisMapper.Select<FormItemIFrames>("bwdf_FormItemIFrames_Select_By_FormItemDefinitionId", formItemDefinitionId);
        }

        /// <summary>
        /// 根据表单定义ID获取 Apps 信息.
        /// </summary>
        /// <returns></returns>
        public IList<FormItemIFrames> SelectByFormDefinitionId(Guid formDefinitionId)
        {
            return IBatisMapper.Select<FormItemIFrames>("bwdf_FormItemIFrames_Select_By_FormdefinitionId", formDefinitionId);
        }

        /// <summary>
        /// 判断指定应用呈现名称是否存在.
        /// </summary>
        /// <param name="formItemDefinitionId"></param>
        /// <param name="activityName"></param>
        /// <param name="settingType"></param>
        /// <returns></returns>
        public bool IsExists(Guid formItemDefinitionId, string activityName, int settingType)
        {
            FormItemIFrames item = new FormItemIFrames();
            item.FormItemDefinitionId = formItemDefinitionId;
            item.ActivityName = activityName;
            item.SettingType = settingType;
            int result = IBatisMapper.Load<int>("bwdf_FormItemIFrames_Select_IsExists", item);
            return result > 0;
        }

        #endregion
    }
}

