using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.IBatisNet
{
    public class DepartmentService : IDepartmentService
    {
        #region IDepartmentService 成员

        public Department GetDepartmentById(string dpId)
        {
            return IBatisMapper.Load<Department>("bw_Depts_Select", dpId);
        }

        public Department GetDepartmentByFullName(string dpFullName)
        {
            IList<Department> depts = IBatisMapper.Select<Department>("bw_Depts_Select_By_FullName", dpFullName);
            return ((depts.Count == 0) ? null : depts[0]);
        }

        public IList<Department> GetDepartmentsByParentId(string parentId)
        {
            return IBatisMapper.Select<Department>("bw_Depts_Select_ByParentId", parentId);
        }

        public IList<Department> GetDepartmentsLikeName(string dpName)
        {
            return IBatisMapper.Select<Department>("bw_Depts_Select_Top_Like_Name", dpName);
        }

        public DataTable GetAllDepartments()
        {
            string sql = @"SELECT DpId, DpName, ParentDpId, DpFullName, DpLevel, DeptOrderNo, IsTmpDp, Type, 
                CreatedTime, LastModTime, Creator, LastModifier FROM bw_Depts";

            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        #endregion
    }
}
