using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.IBatisNet
{
    public class UserService : IUserService
    {
        #region IUserService 成员

        public void InsertUser(UserInfo item)
        {
            IBatisMapper.Insert("bw_Users_Insert", item);
        }

        public int UpdateUser(UserInfo item)
        {
            return IBatisMapper.Update("bw_Users_Update", item);
        }

        public int ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (newPassword == null)
                return -1;
            // 加密密码
            oldPassword = string.IsNullOrEmpty(oldPassword) ? oldPassword : TripleDESHelper.Encrypt(oldPassword);
            newPassword = TripleDESHelper.Encrypt(newPassword);

            Hashtable parameters = new Hashtable();
            parameters.Add("UserName", userName);
            parameters.Add("OldPassword", oldPassword);
            parameters.Add("NewPassword", newPassword);
            return IBatisMapper.Update("bw_Users_ChangePassword", parameters);
        }

        public int DeleteByUserId(Guid userId)
        {
            return IBatisMapper.Delete("bw_Users_Delete", userId);
        }

        public bool UserIsExists(string userName)
        {
            int count = IBatisMapper.Mapper.QueryForObject<int>("bw_Users_Select_IsExists", userName);
            return (count >= 1);
        }

        public UserInfo GetUserByUserId(Guid userId)
        {
            return IBatisMapper.Load<UserInfo>("bw_Users_Select_ByUserId", userId);
        }

        public UserInfo GetUserByUserName(string userName)
        {
            return IBatisMapper.Load<UserInfo>("bw_Users_Select_ByUserName", userName);
        }

        public UserInfo GetUserByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
                throw new ArgumentException("员工号 employeeId 为空.");
            return IBatisMapper.Load<UserInfo>("bw_Users_Select_ByEmployeeId", employeeId);
        }

        public UserInfo GetUserByRealName(string realName, string dpFullName)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("RealName", realName);
            parameters.Add("DpFullName", dpFullName);
            return IBatisMapper.Load<UserInfo>("bw_Users_Select_ByRealName", parameters);
        }

        public IList<UserInfo> GetUsersLikeName(string prefixName)
        {
            return IBatisMapper.Select<UserInfo>("bw_Users_Select_LikeName", prefixName);
        }

        public DataTable GetUsersByPager(string keywords, string dpId, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bw_Users_Detail";
            string fieldKey = "UserId";
            string fieldShow = "UserId, UserName, Password, Email, Mobile, Tel, EmployeeId, RealName, Type, Status, DpId, DpFullName, Ext_Int, Ext_Decimal, Ext_Str1, Ext_Str2, Ext_Str3, CreatedTime, LastModTime, Creator, LastModifier";
            //string fieldOrder = "LastModTime DESC";
            string fieldOrder = "SortOrder ASC";

            StringBuilder where = new StringBuilder();
            where.Append("(1=1)");
            if (!string.IsNullOrEmpty(dpId))
                where.AppendFormat(" AND (DpId LIKE '{0}%')", dpId);
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND ((UserName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Email LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Mobile LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (RealName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (DpFullName LIKE '%{0}%'))", keywords);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        public DataTable GetUsersByPager(string keywords, Guid? roleId, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bw_Users_Detail";
            string fieldKey = "UserId";
            string fieldShow = "UserId, UserName, Password, Email, Mobile, Tel, EmployeeId, RealName, Type, Status, DpId, DpFullName, Ext_Int, Ext_Decimal, Ext_Str1, Ext_Str2, Ext_Str3, CreatedTime, LastModTime, Creator, LastModifier";
            //string fieldOrder = "LastModTime DESC";
            string fieldOrder = "SortOrder ASC";

            StringBuilder where = new StringBuilder();
            where.Append("(1=1)");
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND ((UserName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Email LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (Mobile LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (RealName LIKE '%{0}%')", keywords);
                where.AppendFormat(" OR (DpFullName LIKE '%{0}%'))", keywords);
            }
            if (roleId != null)
            {
                where.AppendFormat(" AND (UserId IN (SELECT bw_UsersInRoles.UserId FROM bw_UsersInRoles WHERE bw_UsersInRoles.RoleId = '{0}'))", roleId.Value);
            }

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        #endregion

        #region IUserService 成员

        public IList<UserInfo> GetUsersByDpId(string dpId)
        {
            return IBatisMapper.Select<UserInfo>("bw_Users_Select_ByDpId", dpId);
        }

        public IList<UserInfo> GetUsersLikeDpId(string dpId)
        {
            return IBatisMapper.Select<UserInfo>("bw_Users_Select_LikeDpId", dpId);
        }

        public int GetUserCountLikeDpId(string dpId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bw_Users_Select_Count_LikeDpId", dpId);
        }

        #endregion
    }
}
