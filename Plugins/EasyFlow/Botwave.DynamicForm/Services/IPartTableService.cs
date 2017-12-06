using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Services
{
    /// <summary>
    /// bwdf_FormItemInstances 分表服务接口.
    /// </summary>
    public interface IPartTableService
    {
        /// <summary>
        /// 创建指定索引的分表，如存在, 则不创建.
        /// </summary>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        bool CreateTable(int tableIndex);

        /// <summary>
        /// 获取指定数目的已完成的表单实例编号列表.
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        IList<Guid> GetCompleteInstanceList(int topCount);

        /// <summary>
        /// 转移指定数目的表单实例的表单项实例数据到指定索引的分表中.
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        bool MigrateData(int topCount, int tableIndex);

        /// <summary>
        /// 转移指定表单实例编号列表的表单项实例数据到指定索引的分表中.
        /// </summary>
        /// <param name="formInstanceIdList"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        bool MigrateData(IList<Guid> formInstanceIdList, int tableIndex);
    }
}
