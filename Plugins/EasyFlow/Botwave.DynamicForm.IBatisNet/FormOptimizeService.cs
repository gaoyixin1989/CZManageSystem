using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;

namespace Botwave.DynamicForm.IBatisNet
{
    /// <summary>
    /// 表单实例内容优化服务实现类.
    /// </summary>
    public class FormOptimizeService : IFormOptimizeService
    {
        #region IFormOptimizeService 成员

        public string GetFormContent(Guid formInstanceId)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwdf_FormInstances_Contents_Select_FormContent", formInstanceId);
        }

        public void SaveFormContent(Guid formInstanceId, string formContent)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("FormInstanceId", formInstanceId);
            parameters.Add("FormContent", formContent);

            // 判断新增还是更新.
            int count = IBatisMapper.Mapper.QueryForObject<int>("bwdf_FormInstances_Contents_Select_Count_ByFormInstanceId", formInstanceId);
            if (count > 0)
                IBatisMapper.Update("bwdf_FormInstances_Contents_Update", parameters);
            else
                IBatisMapper.Insert("bwdf_FormInstances_Contents_Insert", parameters);
        }

        #endregion
    }
}
