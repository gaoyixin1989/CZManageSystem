using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class Uum_Organizationinfo
    {
        /// <summary>
        /// 组织ID(针对APP)
        /// <summary>
        public string OUGUID { get; set; }
        /// <summary>
        /// 父组织ID(针对APP)
        /// <summary>
        public string parentOUGUID { get; set; }
        /// <summary>
        /// 组织ID (portal),兼容portal，现不推荐使用
        /// <summary>
        public Nullable<decimal> OUID { get; set; }
        /// <summary>
        /// 组织类型(UN: 公司  UM: 部门,TD:团队)
        /// <summary>
        public string orgTypeID { get; set; }
        /// <summary>
        /// 组织状态（1 启用  0停用）
        /// <summary>
        public string orgState { get; set; }
        /// <summary>
        /// 组织名称(portal)/团队名称
        /// <summary>
        public string OUName { get; set; }
        /// <summary>
        /// 上级组织ID(portal), 兼容portal，现不推荐使用
        /// <summary>
        public Nullable<decimal> parentOUID { get; set; }
        /// <summary>
        /// 组织全名(portal)
        /// <summary>
        public string OUFullName { get; set; }
        /// <summary>
        /// 组织层次(portal)
        /// <summary>
        public Nullable<decimal> OULevel { get; set; }
        /// <summary>
        /// 组织排序(portal)
        /// <summary>
        public string OUOrder { get; set; }
        /// <summary>
        /// 组织中的DN(portal)
        /// <summary>
        public string OrgDN { get; set; }
        /// <summary>
        /// 所属地区(省公司)
        /// <summary>
        public string region { get; set; }
        /// <summary>
        /// 地区序号
        /// <summary>
        public string regionID { get; set; }
        /// <summary>
        /// 所属公司
        /// <summary>
        public string company { get; set; }
        /// <summary>
        /// 公司序号
        /// <summary>
        public string companyID { get; set; }
        /// <summary>
        /// 所属分公司
        /// <summary>
        public string branch { get; set; }
        /// <summary>
        /// 分公司序号
        /// <summary>
        public string branchID { get; set; }
        /// <summary>
        /// 所属部门
        /// <summary>
        public string department { get; set; }
        /// <summary>
        /// 部门序号
        /// <summary>
        public string departmentID { get; set; }
        /// <summary>
        /// 所属科室
        /// <summary>
        public string userGroup { get; set; }
        /// <summary>
        /// 科室序号
        /// <summary>
        public string userGroupID { get; set; }
        /// <summary>
        /// 服务厅
        /// <summary>
        public string hall { get; set; }
        /// <summary>
        /// 服务厅ID
        /// <summary>
        public string hallid { get; set; }
        /// <summary>
        /// 销售点（班、组）
        /// <summary>
        public string salepoint { get; set; }
        /// <summary>
        /// 销售点ID
        /// <summary>
        public string salepointid { get; set; }
        /// <summary>
        /// 所属组（多个组以英文逗号分隔），如组织是代维团队，则此值为供应商ID和移动接口人，示例：11992233,wuyuming
        ///如组织是渠道树，则值此为渠道编码，示例：ZQFN0014
        /// <summary>
        public string teams { get; set; }
        /// <summary>
        /// 记录创建日期
        /// <summary>
        public Nullable<DateTime> createTime { get; set; }
        /// <summary>
        /// 最后修改日期
        /// <summary>
        public Nullable<DateTime> lastModifyTime { get; set; }

    }
}
