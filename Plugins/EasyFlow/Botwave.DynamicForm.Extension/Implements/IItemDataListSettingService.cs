using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Botwave.DynamicForm.Extension.Domain;

namespace Botwave.DynamicForm.Extension.Implements
{
    public interface IItemDataListSettingService
    {
        /// <summary>
        /// 获取DataList设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataListSetting GetDataListSetting(Guid id);

        /// <summary>
        /// 根据表单定义ID获取DataList设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<DataListSetting> GetDataListSettingByFormDefinitionId(Guid id);

        /// <summary>
        /// 插入DataList设置信息
        /// </summary>
        /// <param name="dataListSetting"></param>
        void DataListSettingInsert(DataListSetting dataListSetting);

        /// <summary>
        /// 更新DataList设置信息
        /// </summary>
        int DataListSettingUpdate(DataListSetting dataListSetting);

        /// <summary>
        /// 删除DataList设置信息
        /// </summary>
        int DataListSettingDelete(Guid id);
    }
}
