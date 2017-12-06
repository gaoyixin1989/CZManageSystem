using System;
using System.Collections.Generic;
using System.Data;
using Botwave.Security.Domain;

namespace Botwave.Security.Service
{
    /// <summary>
    /// 用户服务接口.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void InsertUser(UserInfo item);

        /// <summary>
        /// 更新用户信息.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int UpdateUser(UserInfo item);

        /// <summary>
        /// 修改用户密码.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        int ChangePassword(string userName, string oldPassword, string newPassword);

        /// <summary>
        /// 删除指定用户编号的用户信息.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteByUserId(Guid userId);

        /// <summary>
        /// 检查用户名是否已经存在.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool UserIsExists(string userName);

        /// <summary>
        /// 获取指定用户ID 的用户信息.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserInfo GetUserByUserId(Guid userId);

        /// <summary>
        /// 获取指定用户名的用户信息.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserInfo GetUserByUserName(string userName);

        /// <summary>
        /// 获取指定员工 ID 的用户信息.
        /// </summary>
        /// <param name="employeeId">员工 ID.</param>
        /// <returns></returns>
        UserInfo GetUserByEmployeeId(string employeeId);

        /// <summary>
        /// 获取指定用户真实姓名与所在部门全名的用户信息.
        /// </summary>
        /// <param name="realName">用户真实姓名.</param>
        /// <param name="dpFullName">部门全名.</param>
        /// <returns></returns>
        UserInfo GetUserByRealName(string realName, string dpFullName);

        /// <summary>
        /// 获取指定部门编号的用户列表.
        /// </summary>
        /// <param name="dpId"></param>
        /// <returns></returns>
        IList<UserInfo> GetUsersByDpId(string dpId);

        /// <summary>
        /// 获取匹配指定部分用户名或者姓名的用户列表.
        /// </summary>
        /// <param name="prefixName"></param>
        /// <returns></returns>
        IList<UserInfo> GetUsersLikeName(string prefixName);

        /// <summary>
        /// 获取匹配指定部分部门编号的用户列表.
        /// </summary>
        /// <param name="dpId"></param>
        /// <returns></returns>
        IList<UserInfo> GetUsersLikeDpId(string dpId);

        /// <summary>
        /// 获取匹配指定部分部门编号的用户数.
        /// </summary>
        /// <param name="dpId"></param>
        /// <returns></returns>
        int GetUserCountLikeDpId(string dpId);

        /// <summary>
        /// 分页获取用户列表.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="dpId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetUsersByPager(string keywords, string dpId, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 分页获取用户列表.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetUsersByPager(string keywords, Guid? roleId, int pageIndex, int pageSize, ref int recordCount);
    }
}
