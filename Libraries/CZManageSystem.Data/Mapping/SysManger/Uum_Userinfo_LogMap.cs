using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Uum_Userinfo_LogMap : EntityTypeConfiguration<Uum_Userinfo_Log>
    {
        public Uum_Userinfo_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.employee);
            /// <summary>
            /// 用户帐号/代维人员账号（离职，调出时账号为空）
            /// <summary>
            this.Property(t => t.userID)

          .HasMaxLength(50);
            /// <summary>
            /// 用户全名
            /// <summary>
            this.Property(t => t.fullName)

          .HasMaxLength(100);
            /// <summary>
            /// 编制部门GUID对外提供数据（针对非PORTAL应用）/代维人员为所属团队
            /// <summary>
            this.Property(t => t.OUGUID)

          .HasMaxLength(50);
            /// <summary>
            /// 工作部门OUGUID对外提供数据（针对非PORTAL应用）/代维人员为所属团队
            /// <summary>
            this.Property(t => t.workOUGUID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属地区(省公司) 
            /// <summary>
            this.Property(t => t.region)

          .HasMaxLength(100);
            /// <summary>
            /// 地区序号 (OUGUID数据)
            /// <summary>
            this.Property(t => t.regionID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属公司
            /// <summary>
            this.Property(t => t.company)

          .HasMaxLength(100);
            /// <summary>
            /// 公司序号 (OUGUID数据)
            /// <summary>
            this.Property(t => t.companyID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属分公司 
            /// <summary>
            this.Property(t => t.branch)

          .HasMaxLength(100);
            /// <summary>
            /// 分公司序号 (OUGUID数据)
            /// <summary>
            this.Property(t => t.branchID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属部门
            /// <summary>
            this.Property(t => t.department)

          .HasMaxLength(100);
            /// <summary>
            /// 部门序号 (OUGUID数据)
            /// <summary>
            this.Property(t => t.departmentID)

          .HasMaxLength(50);
            /// <summary>
            /// 所属科室
            /// <summary>
            this.Property(t => t.userGroup)

          .HasMaxLength(100);
            /// <summary>
            /// 科室序号 (OUGUID数据)
            /// <summary>
            this.Property(t => t.userGroupID)

          .HasMaxLength(50);
            /// <summary>
            /// 服务厅
            /// <summary>
            this.Property(t => t.hall)

          .HasMaxLength(100);
            /// <summary>
            /// 服务厅序号
            /// <summary>
            this.Property(t => t.hallid)

          .HasMaxLength(50);
            /// <summary>
            /// 销售点（班、组）
            /// <summary>
            this.Property(t => t.salepoint)

          .HasMaxLength(100);
            /// <summary>
            /// 销售点序号(portal数据)
            /// <summary>
            this.Property(t => t.salepointid)

          .HasMaxLength(50);
            /// <summary>
            /// 因此字段之前留空，现为营业厅人员相关数据存储使用主要存储BOSS工号，BOSS工号类型型等，使用@@@分隔：BOSS工号@@@登陆类型@@@员工类型@@@账号状态@@@ BOSS工号操作员类型 @@@启用时间(时间格式:YYYY-MM-DD)@@@所属地市@@@所属部门
            ///(其中账号状态取值为1和2，1为有效，2为失效)
            ///停用时间请使用(userQuitDate)
            /// <summary>
            this.Property(t => t.teams)

          .HasMaxLength(255);
            /// <summary>
            /// 人员库类型 一般来讲，应用系统只需要有效的用户数据，即jobtype为000001（在岗人员库）000004（社会化员工库）、000009（全省交流人员库）、000010（全省借调人员库）、000088（代维人员库）、000099（临时借调人员库）000003 （异地任职人员库）000002 (离退人员) 000006(离职人员库）000007（内退人员库）
            ///000066(客服人员库）000077（营业厅人员库）000055(系统账号）000044 (公用账号）
            /// <summary>
            this.Property(t => t.jobType)

          .HasMaxLength(20);
            /// <summary>
            /// 人员库类型-PORTAL(参考附录)
            /// <summary>
            this.Property(t => t.userType)

          .HasMaxLength(20);
            /// <summary>
            /// 工作电话
            /// <summary>
            this.Property(t => t.workPhone)

          .HasMaxLength(50);
            /// <summary>
            /// 移动电话
            /// <summary>
            this.Property(t => t.telePhone)

          .HasMaxLength(30);
            /// <summary>
            /// 办公邮件
            /// <summary>
            this.Property(t => t.email)

          .HasMaxLength(128);
            /// <summary>
            /// 联系地址
            /// <summary>
            this.Property(t => t.address)

          .HasMaxLength(255);
            /// <summary>
            /// 性别(参考附录)
            /// <summary>
            this.Property(t => t.sex)

          .HasMaxLength(1);
            /// <summary>
            /// 职位
            /// <summary>
            this.Property(t => t.title)

          .HasMaxLength(80);
            /// <summary>
            /// 集团短号
            /// <summary>
            this.Property(t => t.shortMobile)

          .HasMaxLength(12);
            /// <summary>
            /// 用户职位等级(参考附录)
            /// <summary>
            this.Property(t => t.userPosiLevel)

          .HasMaxLength(2);
            /// <summary>
            /// 套入职级
            /// <summary>
            this.Property(t => t.currentLevel)

          .HasMaxLength(2);
            /// <summary>
            /// 用户职位 (参考附录)
            /// <summary>
            this.Property(t => t.employeeClass)

          .HasMaxLength(50);
            /// <summary>
            /// 员工职称(参考附录)
            /// <summary>
            this.Property(t => t.userGrade)

          .HasMaxLength(100);
            /// <summary>
            /// cmccaccount
            /// <summary>
            this.Property(t => t.CMCCAccount)

          .HasMaxLength(100);
            /// <summary>
            /// 排序ID，在UI上显示时的做排序
            /// <summary>
            this.Property(t => t.orderID)

          .HasMaxLength(255);
            /// <summary>
            /// 民族(参考附录)
            /// <summary>
            this.Property(t => t.userNation)

          .HasMaxLength(5);
            /// <summary>
            /// 政治面貌(参考附录)
            /// <summary>
            this.Property(t => t.userReligion)

          .HasMaxLength(5);
            /// <summary>
            /// 用户DN
            /// <summary>
            this.Property(t => t.userDN)

          .HasMaxLength(255);
            /// <summary>
            /// 职务描述
            /// <summary>
            this.Property(t => t.dutyDesc)

          .HasMaxLength(200);
            /// <summary>
            /// 用户资料在LDAP中变动的时间
            /// <summary>
            this.Property(t => t.changeTime)

          .HasMaxLength(255);
            /// <summary>
            /// 3G电话（现用于代维系统工单号ID）
            /// <summary>
            this.Property(t => t.PHONE3G)

          .HasMaxLength(20);
            /// <summary>
            /// 组织ID(兼容SMAP平台，其他系统不使用)
            /// <summary>
            this.Property(t => t.OrgID)

          .HasMaxLength(50);
            /// <summary>
            /// 日志类型(ADD、UPT、DEL)
            /// <summary>
            this.Property(t => t.operationType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("UUM_USERINFO_LOG");
            // 用户身份唯一标识，2位公司代号+8位员工编号,代维人员为一个10位的数字编码
            this.Property(t => t.employee).HasColumnName("employee");
            // 用户帐号/代维人员账号（离职，调出时账号为空）
            this.Property(t => t.userID).HasColumnName("userID");
            // 用户全名
            this.Property(t => t.fullName).HasColumnName("fullName");
            // 编制部门PortalID数据
            this.Property(t => t.OUID).HasColumnName("OUID");
            // 编制部门GUID对外提供数据（针对非PORTAL应用）/代维人员为所属团队
            this.Property(t => t.OUGUID).HasColumnName("OUGUID");
            // 工作部门OUGUID对外提供数据（针对非PORTAL应用）/代维人员为所属团队
            this.Property(t => t.workOUGUID).HasColumnName("workOUGUID");
            // 所属地区(省公司) 
            this.Property(t => t.region).HasColumnName("region");
            // 地区序号 (OUGUID数据)
            this.Property(t => t.regionID).HasColumnName("regionID");
            // 所属公司
            this.Property(t => t.company).HasColumnName("company");
            // 公司序号 (OUGUID数据)
            this.Property(t => t.companyID).HasColumnName("companyID");
            // 所属分公司 
            this.Property(t => t.branch).HasColumnName("branch");
            // 分公司序号 (OUGUID数据)
            this.Property(t => t.branchID).HasColumnName("branchID");
            // 所属部门
            this.Property(t => t.department).HasColumnName("department");
            // 部门序号 (OUGUID数据)
            this.Property(t => t.departmentID).HasColumnName("departmentID");
            // 所属科室
            this.Property(t => t.userGroup).HasColumnName("userGroup");
            // 科室序号 (OUGUID数据)
            this.Property(t => t.userGroupID).HasColumnName("userGroupID");
            // 服务厅
            this.Property(t => t.hall).HasColumnName("hall");
            // 服务厅序号
            this.Property(t => t.hallid).HasColumnName("hallid");
            // 销售点（班、组）
            this.Property(t => t.salepoint).HasColumnName("salepoint");
            // 销售点序号(portal数据)
            this.Property(t => t.salepointid).HasColumnName("salepointid");
            // 因此字段之前留空，现为营业厅人员相关数据存储使用主要存储BOSS工号，BOSS工号类型型等，使用@@@分隔：BOSS工号@@@登陆类型@@@员工类型@@@账号状态@@@ BOSS工号操作员类型 @@@启用时间(时间格式:YYYY-MM-DD)@@@所属地市@@@所属部门
            //(其中账号状态取值为1和2，1为有效，2为失效)
            //停用时间请使用(userQuitDate)

            this.Property(t => t.teams).HasColumnName("teams");
            // 人员库类型 一般来讲，应用系统只需要有效的用户数据，即jobtype为000001（在岗人员库）000004（社会化员工库）、000009（全省交流人员库）、000010（全省借调人员库）、000088（代维人员库）、000099（临时借调人员库）000003 （异地任职人员库）000002 (离退人员) 000006(离职人员库）000007（内退人员库）
            //000066(客服人员库）000077（营业厅人员库）000055(系统账号）000044(公用账号）

            this.Property(t => t.jobType).HasColumnName("jobType");
            // 人员库类型-PORTAL(参考附录)
            this.Property(t => t.userType).HasColumnName("userType");
            // 工作电话
            this.Property(t => t.workPhone).HasColumnName("workPhone");
            // 移动电话
            this.Property(t => t.telePhone).HasColumnName("telePhone");
            // 办公邮件
            this.Property(t => t.email).HasColumnName("email");
            // 联系地址
            this.Property(t => t.address).HasColumnName("address");
            // 生日
            this.Property(t => t.userBirthday).HasColumnName("userBirthday");
            // 性别(参考附录)
            this.Property(t => t.sex).HasColumnName("sex");
            // 职位
            this.Property(t => t.title).HasColumnName("title");
            // 集团短号
            this.Property(t => t.shortMobile).HasColumnName("shortMobile");
            // 用户职位等级(参考附录)
            this.Property(t => t.userPosiLevel).HasColumnName("userPosiLevel");
            // 套入职级
            this.Property(t => t.currentLevel).HasColumnName("currentLevel");
            // 用户职位 (参考附录)
            this.Property(t => t.employeeClass).HasColumnName("employeeClass");
            // 员工职称(参考附录)
            this.Property(t => t.userGrade).HasColumnName("userGrade");
            // cmccaccount
            this.Property(t => t.CMCCAccount).HasColumnName("CMCCAccount");
            // 排序ID，在UI上显示时的做排序
            this.Property(t => t.orderID).HasColumnName("orderID");
            // 加入公司日期
            this.Property(t => t.userJoinInDate).HasColumnName("userJoinInDate");
            // 民族(参考附录)
            this.Property(t => t.userNation).HasColumnName("userNation");
            // 政治面貌(参考附录)
            this.Property(t => t.userReligion).HasColumnName("userReligion");
            // 离开公司日期（控制用户账号使用的辅助字段）
            this.Property(t => t.userQuitDate).HasColumnName("userQuitDate");
            // 用户DN
            this.Property(t => t.userDN).HasColumnName("userDN");
            // 职务描述
            this.Property(t => t.dutyDesc).HasColumnName("dutyDesc");
            // 用户资料在LDAP中变动的时间
            this.Property(t => t.changeTime).HasColumnName("changeTime");
            // 记录创建日期
            this.Property(t => t.createTime).HasColumnName("createTime");
            // 最后修改日期
            this.Property(t => t.lastModifyTime).HasColumnName("lastModifyTime");
            // 3G电话（现用于代维系统工单号ID）
            this.Property(t => t.PHONE3G).HasColumnName("PHONE3G");
            // 组织ID(兼容SMAP平台，其他系统不使用)
            this.Property(t => t.OrgID).HasColumnName("OrgID");
            // 日志ID
            this.Property(t => t.logID).HasColumnName("logID");
            // 日志类型(ADD、UPT、DEL)
            this.Property(t => t.operationType).HasColumnName("operationType");
        }
    }
}
