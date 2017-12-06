using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
    public class EquipApp
    {
        public int Id { get; set; }
        public string ApplyId { get; set; }
        public string ApplyName { get; set; }
        public string Deptname { get; set; }
        public string Job { get; set; }
        public Nullable<DateTime> ApplyTime { get; set; }
        public string ApplyTitle { get; set; }
        public string ApplySn { get; set; }
        public string Nature { get; set; }
        public string Tel { get; set; }
        public string EquipClass { get; set; }
        public string ApplyReason { get; set; }
        public string Chief { get; set; }
        public Nullable<int> AppNum { get; set; }
        public string EquipInfo { get; set; }
        public string ProjSn { get; set; }
        public string AssetSn { get; set; }
        public string OutFlag { get; set; }
        public Nullable<int> StockType { get; set; }
        public string BUsername { get; set; }
        public string Remark { get; set; }
        public string Editor { get; set; }
        public Nullable<DateTime> EditTime { get; set; }
        public string CancleReason { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<Guid> WorkflowInstanceId { get; set; }

    }
    public class EquipAppQueryBuilder
    {

        /// <summary>
        /// �豸����
        /// </summary>
        public string EquipClass { get; set; }
        /// <summary>
        /// Ͷ����Ŀ���
        /// </summary>
        public string ProjSn { get; set; }
        /// <summary>
        /// �豸�ͺ�
        /// </summary>
        public string EquipInfo { get; set; }
        /// <summary>
        /// �ʲ�����
        /// </summary>
        public Nullable<int> StockType { get; set; }
        /// <summary>
        /// ���״̬
        /// </summary>
        public Nullable<int> StockStatus { get; set; }
        public string ApplyTitle { get; set; }
        public string ApplyName { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        public int state { get; set; }

    }
        public class OutstockQueryBuilder
    {
        public string ApplySn { get; set; }
        public string ApplyName { get; set; }
        public DateTime? Createdtime_Start { get; set; }
        public DateTime? Createdtime_End { get; set; }
    }
    /// <summary>
    /// �豸����
    /// </summary>
    public class OutStock
    {
        public string EquipClass { get; set; }
        public string ProjSn { get; set; }
        public string ProjName { get; set; }
        public string EquipInfo { get; set; }
        public int? StockType { get; set; }
        public DateTime? EditTime { get; set; }
        public int? totalnum { get; set; }

    }
    /// <summary>
    /// �豸 �ʲ�����
    /// </summary>
    public class EquipAssetQueryBuilder
    {
        public string EquipClass { get; set; }
        public string ProjSn { get; set; }
        public string EquipInfo { get; set; }
        public string StockStatus { get; set; }
        public string AssetSn { get; set; }

    }



    public class EquipStcock
    {
        public Guid WorkflowInstanceId { get; set; }
        public int Id { get; set; }
        public string ApplyId { get; set; }
        public Nullable<int> Editor { get; set; }
        public DateTime EditTime { get; set; }
        public string ApplyTitle { get; set; }
        /// <summary>
        /// ���̵���
        /// </summary>
		public string ApplySn { get; set; }
        /// <summary>
        /// ��λְ��
        /// </summary>
        public string Job { get; set; }
        public string EquipClass { get; set; }
        public string ApplyReason { get; set; }
        public string Chief { get; set; }
        public Nullable<int> AppNum { get; set; }
        public string EquipInfo { get; set; }
        public string ProjSn { get; set; }
        public string AssetSn { get; set; }
        public string OutFlag { get; set; }
        public string BUsername { get; set; }
        public string Deptname { get; set; }
        public string ApplyName { get; set; }
        public DateTime ApplyTime { get; set; }
        public string Tel { get; set; }
        public int enternum { get; set; }
        public int outnum { get; set; }
        public int totalnum { get; set; }

    }
}
