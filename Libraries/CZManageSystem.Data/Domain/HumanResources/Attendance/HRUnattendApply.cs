using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class HRUnattendApply
    {
        /// <summary>
        /// 主键
        /// <summary>
        public Guid ApplyID { get; set; }
        /// <summary>
        /// 标题
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// 流程实例Id
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public string ApplySn { get; set; }
        /// <summary>
        /// 申请人
        /// <summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 开始时间
        /// <summary>
        public Nullable<DateTime> StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// <summary>
        public Nullable<DateTime> EndTime { get; set; }
        /// <summary>
        /// 天数
        /// <summary>
        public Nullable<decimal> PeriodTime { get; set; }
        /// <summary>
        /// 创建时间、申请时间
        /// <summary>
        public Nullable<DateTime> CreateTime { get; set; }
        /// <summary>
        /// 申请单位
        /// <summary>
        public string ApplyUnit { get; set; }
        /// <summary>
        /// 所属部门
        /// <summary>
        public string ApplyDept { get; set; }
        /// <summary>
        /// 联系号码
        /// <summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 申请原因
        /// <summary>
        public string Reason { get; set; }
        /// <summary>
        /// 异常记录
        /// <summary>
        public string RecordContent { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// 附件IDs
        /// <summary>
        public string AccessoryIds { get; set; }
        /// <summary>
        /// 职位？
        /// <summary>
        public string UnattendPost { get; set; }
        /// <summary>
        /// 考勤ID
        /// <summary>
        public string AttendanceIds { get; set; }
        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
    }
}
