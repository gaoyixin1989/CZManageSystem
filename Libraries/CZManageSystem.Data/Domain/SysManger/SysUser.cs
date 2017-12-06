using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysUser : BaseEntity<int>
    {
        public SysUser()
        {
            this.SysUserRoles = new List<SysUserRole>();
        }
        /// <summary>
        /// 获取用户的guid，不映射表，仅做cookie的key
        /// </summary>
        public Guid UserGuid { get { return Guid.NewGuid(); } }
        /// <summary>
        /// 获取或设置用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 获取或设置登录账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 获取或设置登录密码
        /// </summary>
        public string LoginPWD { get; set; }
        /// <summary>
        /// 获取或设置部门编号
        /// </summary>
        //public virtual SysDeptment sysDeptment { get; set; }
        /// <summary>
        /// 获取或设置邮箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 获取或设置手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        ///  获取或设置电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 获取或设置工号
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 获取或设置职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 获取或设置入职日期
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 是否为外部人员
        /// </summary>
        public bool IsOuter { get; set; }
        /// <summary>
        /// 获取或设置删除状态
        /// </summary>
        public bool DelFlag { get; set; }
        /// <summary>
        /// 获取或设置备注
        /// </summary>
        public string Remark { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }
    }
}
