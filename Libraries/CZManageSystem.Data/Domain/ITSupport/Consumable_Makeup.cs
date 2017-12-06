using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.ITSupport
{
    public class Consumable_Makeup
    {
        /// <summary>
        /// 编号
        /// <summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 流程实例ID
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        /// <summary>
        /// 流程单号
        /// <summary>
        public string Series { get; set; }
        /// <summary>
        /// 申请提交时间
        /// <summary>
        public Nullable<DateTime> AppTime { get; set; }
        /// <summary>
        /// 申请部门id
        /// <summary>
        public string AppDept { get; set; }
        /// <summary>
        /// 申请人
        /// <summary>
        public Nullable<Guid> AppPerson { get; set; }
        /// <summary>
        /// 申请人手机
        /// <summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 申请标题
        /// <summary>
        public string Title { get; set; }
        /// <summary>
        /// 退库原因
        /// <summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态：0保存、1提交
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 使用人ID
        /// <summary>
        public Nullable<Guid> UsePerson { get; set; }
        
                
        public virtual Users UsersForApp { get; set; }//申请人外键
        public virtual Users UsersForUse { get; set; }//使用人外键
        public virtual Depts DeptsForApp { get; set; }//申请人部门外键
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }

    }
}
