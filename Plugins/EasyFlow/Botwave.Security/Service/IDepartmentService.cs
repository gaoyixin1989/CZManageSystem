using System;
using System.Collections.Generic;
using System.Data;
using Botwave.Security.Domain;

namespace Botwave.Security.Service
{
    /// <summary>
    /// 部门数据服务接口.
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// 获取指定部门编号的部门信息.
        /// </summary>
        /// <param name="dpId"></param>
        /// <returns></returns>
        Department GetDepartmentById(string dpId);

        /// <summary>
        /// 获取指定部门全名的部门信息.
        /// </summary>
        /// <param name="dpFullName"></param>
        /// <returns></returns>
        Department GetDepartmentByFullName(string dpFullName);

        /// <summary>
        /// 获取指定上级部门ID 的部门列表.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IList<Department> GetDepartmentsByParentId(string parentId);

        /// <summary>
        /// 获取指定部门名称部分文字的部门列表.
        /// </summary>
        /// <param name="dpName"></param>
        /// <returns></returns>
        IList<Department> GetDepartmentsLikeName(string dpName);

        /// <summary>
        /// 获取全部的部门信息.
        /// </summary>
        /// <returns></returns>
        DataTable GetAllDepartments();
    }
}
