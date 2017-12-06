using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Util
{
    /// <summary>
    /// 广州移动组织部门辅助类.
    /// </summary>
    public static class GmccDeptHelper
    {
        /// <summary>
        /// 根据组织全名来获取部门名称.
        /// </summary>
        /// <param name="dpFullName">组织全名.</param>
        /// <returns></returns>
        public static string GetDeptNameByDpFullName(string dpFullName)
        {
            string deptName = String.Empty;
            if (String.IsNullOrEmpty(dpFullName))
            {
                return deptName;
            }

            int begin = dpFullName.IndexOf('>');
            if (begin == -1)
            {
                return deptName;
            }

            begin = begin + 1;
            int end = dpFullName.IndexOf('>', begin);

            if (end == -1)
            {
                deptName = dpFullName.Substring(begin);
            }
            else
            {
                deptName = dpFullName.Substring(begin, end - begin);
            }
            return deptName;
        }

        /// <summary>
        /// 根据组织Id获取部门Id.
        /// </summary>
        /// <param name="dpId">组织Id.</param>
        /// <returns></returns>
        public static string GetDeptIdByDpId(string dpId)
        {
            //广州部门编码长度为15，科室为17，四分公司的相应地需要减2
            return GetPrefixByDpId(dpId, 15);
        }

        /// <summary>
        /// 根据组织Id获取科室Id.
        /// </summary>
        /// <param name="dpId">组织Id.</param>
        /// <returns></returns>
        public static string GetRoomIdByDpId(string dpId)
        {
            return GetPrefixByDpId(dpId, 17);
        }

        /// <summary>
        /// 获取指定部门的前缀字符串.
        /// </summary>
        /// <param name="dpId"></param>
        /// <param name="lengthOfPrefix"></param>
        /// <returns></returns>
        private static string GetPrefixByDpId(string dpId, int lengthOfPrefix)
        {
            if (String.IsNullOrEmpty(dpId))
            {
                return String.Empty;
            }

            //广州部门编码长度为15，科室为17，四分公司的相应地需要减2
            int lengthOfDpId = dpId.Length;
            if (!dpId.StartsWith("34920440002"))	//如果不属于 广州移动通信公司
            {
                return String.Empty;
            }

            if (dpId.StartsWith("3492044000201"))   //属于广州移动本部
            {
                if (lengthOfDpId < lengthOfPrefix)
                {
                    return String.Empty;
                }
                return dpId.Substring(0, lengthOfPrefix);
            }
            else//四分公司
            {
                if (lengthOfDpId < (lengthOfPrefix - 2))
                {
                    return String.Empty;
                }
                return dpId.Substring(0, (lengthOfPrefix - 2));
            }
        }
    }
}
