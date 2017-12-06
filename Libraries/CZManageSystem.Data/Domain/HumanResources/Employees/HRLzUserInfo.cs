using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
    public class HRLzUserInfo
    {
        public string EmployeeId { get; set; }
        /// <summary>
        /// ְλְ��
        /// <summary>
        public string PositionRank { get; set; }
        /// <summary>
        /// ����ְ��
        /// <summary>
        public string SetIntoTheRanks { get; set; }
        /// <summary>
        /// ��λֵ
        /// <summary>
        public Nullable<int> Tantile { get; set; }
        public Nullable<Guid> UserId { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> LastModTime { get; set; }
        /// <summary>
        /// �޸���
        /// <summary>
        public string LastModFier { get; set; }
        /// <summary>
        /// ��λ
        /// <summary>
        public string Gears { get; set; }
        public virtual Users  Users { get; set; }
    }
}
