using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    public interface IDataDictionaryService : IBaseService<DataDictionary>
    {
        /// <summary>
        /// 获取字典名称分组后的名称列表
        /// </summary>
        /// <returns></returns>
        IList<string> GetDictNameGroup();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="count">数据总数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="DDName">字典名称</param>
        /// <param name="searchDDName">字典名称（模糊查询）</param>
        /// <returns></returns>
        IList<DataDictionary> QueryDataByPage(out int count, int pageIndex = 0, int pageSize = int.MaxValue, string DDName = null, string searchDDName = null);
    }
}