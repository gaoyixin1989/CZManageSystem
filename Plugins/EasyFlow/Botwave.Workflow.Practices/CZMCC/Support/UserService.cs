using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.XQP.Util;

namespace Botwave.Workflow.Practices.CZMCC.Support
{
    public class UserService
    {
        public static readonly UserService Current = new UserService();

        public UserService()
        {

        }

        /// <summary>
        /// 获取无销售精英的用户信息.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="dpId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetUsersByPager(string keywords, string dpId, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bw_Users_Detail";
            string fieldKey = "UserId";
            string fieldShow = "UserId, UserName, Password, Email, Mobile, Tel, EmployeeId, RealName, Type, Status, DpId, DpFullName, Ext_Int, Ext_Decimal, Ext_Str1, Ext_Str2, Ext_Str3, CreatedTime, LastModTime, Creator, LastModifier";
            string fieldOrder = "SortOrder, [Type], UserName";

            StringBuilder where = new StringBuilder();
            where.Append("(UserName NOT LIKE 'XSJY_%') AND (Status >= 0)");
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
            //由从0开始的页码改为从1开始

            pageIndex++;

            IDbDataParameter[] paramSet = IBatisDbHelper.CreateParameterSet(8);

            paramSet[0].ParameterName = "@TableName";
            paramSet[0].Value = tableName;

            paramSet[1].ParameterName = "@PageIndex";
            paramSet[1].Value = pageIndex;
            paramSet[2].ParameterName = "@PageSize";
            paramSet[2].Value = pageSize;
            paramSet[3].ParameterName = "@GetFields";
            paramSet[3].Value = fieldShow;
            paramSet[5].ParameterName = "@OrderField";
            paramSet[5].Value = fieldOrder;
            paramSet[6].ParameterName = "@WhereCondition";
            paramSet[6].Value = where.ToString();
            paramSet[7].ParameterName = "@RecordCount";
            paramSet[7].Direction = ParameterDirection.InputOutput;
            paramSet[7].Value = recordCount;
            paramSet[4].ParameterName = "@GroupBy";
            paramSet[4].Value = "";

            DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_PageList2005", paramSet);
            recordCount = Convert.ToInt32(paramSet[7].Value);

            return ds.Tables[0];
            //return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }

        public DataTable GetUsersByPager(string keywords, Guid? roleId, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bw_Users_Detail";
            string fieldKey = "UserId";
            string fieldShow = "UserId, UserName, Password, Email, Mobile, Tel, EmployeeId, RealName, Type, Status, DpId, DpFullName, Ext_Int, Ext_Decimal, Ext_Str1, Ext_Str2, Ext_Str3, CreatedTime, LastModTime, Creator, LastModifier";
            string fieldOrder = "SortOrder, [Type], UserName";

            StringBuilder where = new StringBuilder();
            where.Append("(Status >= 0)");
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

        public DataTable GetDeptByPager( string keywords, string dpId, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bw_Depts_Detail";
            string fieldKey = "DpId";
            string fieldShow = "DpId, DpName, ParentDpId, DpFullName, DpLevel, DeptOrderNo, IsTmpDp, Type,CreatedTime, LastModTime, Creator, LastModifier";
            string fieldOrder = "OUOrder, DpId";

            StringBuilder where = new StringBuilder();
            where.Append("([Type] =1)");

            if (!string.IsNullOrEmpty(dpId))
            {
                where.AppendFormat(" AND (DpId='{0}')", dpId);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                where.AppendFormat(" AND (DpFullName LIKE '%{0}%')", keywords);
            }
            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where.ToString(), ref recordCount);
        }
    }
}
