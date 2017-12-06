using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm.Extension.Domain;
using Botwave.DynamicForm.Extension.Implements;

namespace Botwave.DynamicForm.Extension.Contracts
{
    /// <summary>
    /// DataList的设置 业务类
    /// </summary>
    public class ItemDataListService : IItemDataListSettingService
    {
        public DataListSetting GetDataListSetting(Guid id)
        {
            return IBatisMapper.Load<DataListSetting>("bwdf_DataListSetting_Select", id);
        }

        public IList<DataListSetting> GetDataListSettingByFormDefinitionId(Guid id)
        {
            return IBatisMapper.Select<DataListSetting>("bwdf_DataListItemDefinition_Select_By_FormdefinitionId", id);
        }

        public void DataListSettingInsert(DataListSetting dataListSetting)
        {
            IBatisMapper.Insert("bwdf_DataListSetting_Insert", dataListSetting);
        }

        public int DataListSettingUpdate(DataListSetting dataListSetting)
        {
            return IBatisMapper.Update("bwdf_DataListSetting_Update", dataListSetting);
        }

        public int DataListSettingDelete(Guid id)
        {
            return IBatisMapper.Delete("bwdf_DataListSetting_Delete", id);
        }
    }
}
