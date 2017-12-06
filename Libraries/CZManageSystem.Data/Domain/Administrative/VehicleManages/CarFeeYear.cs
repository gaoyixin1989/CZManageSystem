using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
    public class CarFeeYear
    {
        /// <summary>
        /// ����
        /// <summary>
        [Required(ErrorMessage = "��������Ϊ��")]
        public Guid CarFeeYearId { get; set; }
        /// <summary>
        /// �༭��ID
        /// <summary>
        [Required(ErrorMessage = "�༭��ID����Ϊ��")]
        public Nullable<Guid> EditorId { get; set; }
        /// <summary>
        /// �༭����
        /// <summary>
        [Required(ErrorMessage = "�༭���ڲ���Ϊ��")]
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// ������λ
        /// <summary>
        [Required(ErrorMessage = "�Ҳ�����Ӧ��������λID")]
        public Nullable<int> CorpId { get; set; }
        /// <summary>
        /// ʹ�õ�λ
        /// <summary>
        [Required(ErrorMessage = "ʹ�õ�λ����Ϊ��")]
        public string CorpName { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        [MinLength (10,ErrorMessage = "�Ҳ�����Ӧ�ĳ���ID")]
        public Nullable<Guid> CarId { get; set; }
        /// <summary>
        /// �ɷ�����
        /// <summary>
        public Nullable<DateTime> PayTime { get; set; }
        /// <summary>
        /// ����С��
        /// <summary>
        public Nullable<decimal> TotalFee { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        [Required(ErrorMessage = "�����˲���Ϊ��")]
        public string Person { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        public virtual CarInfo CarInfo { get; set; }

    }
}
