using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.Composite
{
    public class InnerVoicesQueryBuilder
    {
        public string Applytitle { get; set; }
        public string Creator { get; set; }
        public string DeptName { get; set; }
        public List<string> Themetype { get; set; }
        public DateTime? CreateTime_Start { get; set; }
        public DateTime? CreateTime_End { get; set; }
        public string IsNiming { get; set; }
        public bool? isSumbit { get; set; }
    }

    public partial class InnerVoices
    {
        public int Id { get; set; }
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        public string Applytitle { get; set; }
        public string Applysn { get; set; }
        /// <summary>
        /// 申请人名称
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        public string Creatorid { get; set; }
        public string Themetype { get; set; }
        public string Content { get; set; }
        public string IsNiming { get; set; }
        public string Attids { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public string IsInfo { get; set; }
        public string Username { get; set; }
        public string DeptName { get; set; }
        public string Phone { get; set; }
        public string IsManager { get; set; }

        public virtual Tracking_Workflow TrackingWorkflow { get;set;}

    }
}
