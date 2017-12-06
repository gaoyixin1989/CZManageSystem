using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class Uum_Userinfo_Log
    {
        /// <summary>
        /// 用户身份唯一标识，2位公司代号+8位员工编号,代维人员为一个10位的数字编码
        /// <summary>
        public string employee { get; set; }
        /// <summary>
        /// 用户帐号/代维人员账号（离职，调出时账号为空）
        /// <summary>
        public string userID { get; set; }
        /// <summary>
        /// 用户全名
        /// <summary>
        public string fullName { get; set; }
        /// <summary>
        /// 编制部门PortalID数据
        /// <summary>
        public Nullable<decimal> OUID { get; set; }
        /// <summary>
        /// 编制部门GUID对外提供数据（针对非PORTAL应用）/代维人员为所属团队
        /// <summary>
        public string OUGUID { get; set; }
        /// <summary>
        /// 工作部门OUGUID对外提供数据（针对非PORTAL应用）/代维人员为所属团队
        /// <summary>
        public string workOUGUID { get; set; }
        /// <summary>
        /// 所属地区(省公司) 
        /// <summary>
        public string region { get; set; }
        /// <summary>
        /// 地区序号 (OUGUID数据)
        /// <summary>
        public string regionID { get; set; }
        /// <summary>
        /// 所属公司
        /// <summary>
        public string company { get; set; }
        /// <summary>
        /// 公司序号 (OUGUID数据)
        /// <summary>
        public string companyID { get; set; }
        /// <summary>
        /// 所属分公司 
        /// <summary>
        public string branch { get; set; }
        /// <summary>
        /// 分公司序号 (OUGUID数据)
        /// <summary>
        public string branchID { get; set; }
        /// <summary>
        /// 所属部门
        /// <summary>
        public string department { get; set; }
        /// <summary>
        /// 部门序号 (OUGUID数据)
        /// <summary>
        public string departmentID { get; set; }
        /// <summary>
        /// 所属科室
        /// <summary>
        public string userGroup { get; set; }
        /// <summary>
        /// 科室序号 (OUGUID数据)
        /// <summary>
        public string userGroupID { get; set; }
        /// <summary>
        /// 服务厅
        /// <summary>
        public string hall { get; set; }
        /// <summary>
        /// 服务厅序号
        /// <summary>
        public string hallid { get; set; }
        /// <summary>
        /// 销售点（班、组）
        /// <summary>
        public string salepoint { get; set; }
        /// <summary>
        /// 销售点序号(portal数据)
        /// <summary>
        public string salepointid { get; set; }
        /// <summary>
        /// 因此字段之前留空，现为营业厅人员相关数据存储使用主要存储BOSS工号，BOSS工号类型型等，使用@@@分隔：BOSS工号@@@登陆类型@@@员工类型@@@账号状态@@@ BOSS工号操作员类型 @@@启用时间(时间格式:YYYY-MM-DD)@@@所属地市@@@所属部门
        ///(其中账号状态取值为1和2，1为有效，2为失效)
        ///停用时间请使用(userQuitDate)
        /// <summary>
        public string teams { get; set; }
        /// <summary>
        /// 人员库类型 一般来讲，应用系统只需要有效的用户数据，即jobtype为000001（在岗人员库）000004（社会化员工库）、000009（全省交流人员库）、000010（全省借调人员库）、000088（代维人员库）、000099（临时借调人员库）000003 （异地任职人员库）000002 (离退人员) 000006(离职人员库）000007（内退人员库）
        ///000066(客服人员库）000077（营业厅人员库）000055(系统账号）000044 (公用账号）
        /// <summary>
        public string jobType { get; set; }
        /// <summary>
        /// 人员库类型-PORTAL(参考附录)
        /// <summary>
        public string userType { get; set; }
        /// <summary>
        /// 工作电话
        /// <summary>
        public string workPhone { get; set; }
        /// <summary>
        /// 移动电话
        /// <summary>
        public string telePhone { get; set; }
        /// <summary>
        /// 办公邮件
        /// <summary>
        public string email { get; set; }
        /// <summary>
        /// 联系地址
        /// <summary>
        public string address { get; set; }
        /// <summary>
        /// 生日
        /// <summary>
        public Nullable<DateTime> userBirthday { get; set; }
        /// <summary>
        /// 性别(参考附录)
        /// <summary>
        public string sex { get; set; }
        /// <summary>
        /// 职位
        /// <summary>
        public string title { get; set; }
        /// <summary>
        /// 集团短号
        /// <summary>
        public string shortMobile { get; set; }
        /// <summary>
        /// 用户职位等级(参考附录)
        /// <summary>
        public string userPosiLevel { get; set; }
        /// <summary>
        /// 套入职级
        /// <summary>
        public string currentLevel { get; set; }
        /// <summary>
        /// 用户职位 (参考附录)
        /// <summary>
        public string employeeClass { get; set; }
        /// <summary>
        /// 员工职称(参考附录)
        /// <summary>
        public string userGrade { get; set; }
        /// <summary>
        /// cmccaccount
        /// <summary>
        public string CMCCAccount { get; set; }
        /// <summary>
        /// 排序ID，在UI上显示时的做排序
        /// <summary>
        public string orderID { get; set; }
        /// <summary>
        /// 加入公司日期
        /// <summary>
        public Nullable<DateTime> userJoinInDate { get; set; }
        /// <summary>
        /// 民族(参考附录)
        /// <summary>
        public string userNation { get; set; }
        /// <summary>
        /// 政治面貌(参考附录)
        /// <summary>
        public string userReligion { get; set; }
        /// <summary>
        /// 离开公司日期（控制用户账号使用的辅助字段）
        /// <summary>
        public Nullable<DateTime> userQuitDate { get; set; }
        /// <summary>
        /// 用户DN
        /// <summary>
        public string userDN { get; set; }
        /// <summary>
        /// 职务描述
        /// <summary>
        public string dutyDesc { get; set; }
        /// <summary>
        /// 用户资料在LDAP中变动的时间
        /// <summary>
        public string changeTime { get; set; }
        /// <summary>
        /// 记录创建日期
        /// <summary>
        public Nullable<DateTime> createTime { get; set; }
        /// <summary>
        /// 最后修改日期
        /// <summary>
        public Nullable<DateTime> lastModifyTime { get; set; }
        /// <summary>
        /// 3G电话（现用于代维系统工单号ID）
        /// <summary>
        public string PHONE3G { get; set; }
        /// <summary>
        /// 组织ID(兼容SMAP平台，其他系统不使用)
        /// <summary>
        public string OrgID { get; set; }
        /// <summary>
        /// 日志ID
        /// <summary>
        public Nullable<decimal> logID { get; set; }
        /// <summary>
        /// 日志类型(ADD、UPT、DEL)
        /// <summary>
        public string operationType { get; set; }

    }
}
