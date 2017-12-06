using CZManageSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class SysDeptment: BaseEntity<int>
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        //public int DpId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DpName { get; set; }
        /// <summary>
        /// 所属父级编号
        /// </summary>
        public string ParentDpId { get; set; }
        /// <summary>
        /// 部门全名，如“公司>部门>室”。
        /// </summary>
        public string DpFullName { get; set; }
        /// <summary>
        /// 部门层级
        /// </summary>
        public Nullable<int> DpLevel { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public Nullable<int> OrderNo { get; set; }
        /// <summary>
        /// 是否第三方
        /// </summary>
        public Nullable<bool> IsOuter { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}