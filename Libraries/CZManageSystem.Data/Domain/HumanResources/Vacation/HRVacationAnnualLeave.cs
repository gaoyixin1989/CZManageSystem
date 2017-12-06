using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
    public class HRVacationAnnualLeave
    {
        /// <summary>
        /// ����ID
        /// <summary>
        public Guid ID { get; set; }
        /// <summary>
        /// �û�ID
        /// <summary>
        public Nullable<Guid> UserID { get; set; }
        public Nullable<Guid> ComID { get; set; }
        public string YearDate { get; set; }
        public Nullable<decimal> AnnualLeave { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        /// <summary>
        /// ������ID
        /// <summary>
        public Nullable<Guid> CreateID { get; set; }
        /// <summary>
        /// ʹ������
        /// <summary>
        public Nullable<DateTime> UseDate { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }

        /// <summary>
        /// ���뵥ID
        /// </summary>
        public Nullable<Guid> AppID { get; set; }
        /// <summary>
        /// ���ݼ�����
        /// <summary>
        public Nullable<decimal> SpendDays { get; set; }
        /// <summary>
        /// ��ʼʱ��
        /// <summary>
        public Nullable<DateTime> StartTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> EndTime { get; set; }
        public string Toflag { get; set; }

    }

}
