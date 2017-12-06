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
        /// ����
        /// <summary>
        public Guid ApplyID { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// ����ʵ��Id
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public string ApplySn { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// ��ʼʱ��
        /// <summary>
        public Nullable<DateTime> StartTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> EndTime { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public Nullable<decimal> PeriodTime { get; set; }
        /// <summary>
        /// ����ʱ�䡢����ʱ��
        /// <summary>
        public Nullable<DateTime> CreateTime { get; set; }
        /// <summary>
        /// ���뵥λ
        /// <summary>
        public string ApplyUnit { get; set; }
        /// <summary>
        /// ��������
        /// <summary>
        public string ApplyDept { get; set; }
        /// <summary>
        /// ��ϵ����
        /// <summary>
        public string Mobile { get; set; }
        /// <summary>
        /// ����ԭ��
        /// <summary>
        public string Reason { get; set; }
        /// <summary>
        /// �쳣��¼
        /// <summary>
        public string RecordContent { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// ����IDs
        /// <summary>
        public string AccessoryIds { get; set; }
        /// <summary>
        /// ְλ��
        /// <summary>
        public string UnattendPost { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        public string AttendanceIds { get; set; }
        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
    }
}
