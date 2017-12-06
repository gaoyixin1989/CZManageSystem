using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative
{
    public class BoardroomInfoQueryBuilder
    {
        public int[] BoardroomID { get; set; }//会议室
        public string[] State { get; set; }//状态
        public int? MaxMan_min { get; set; }//最大人数_下限
        public int? MaxMan_max { get; set; }//最大人数_上限

        public string Address { get; set; }//地点
        public string Code { get; set; }//编码
        public string Name { get; set; }//名称
    }

    public class BoardroomInfo
	{
		/// <summary>
		/// 会议室ID
		/// <summary>
		public int BoardroomID { get; set;}
		/// <summary>
		/// 编辑人
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 所属单位
		/// <summary>
		public Nullable<int> CorpID { get; set;}
		/// <summary>
		/// 编号
		/// <summary>
		public string Code { get; set;}
		/// <summary>
		/// 名称
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// 地点
		/// <summary>
		public string Address { get; set;}
		/// <summary>
		/// 最大人数
		/// <summary>
		public Nullable<int> MaxMan { get; set;}
		/// <summary>
		/// 设备
		/// <summary>
		public string Equip { get; set;}
		/// <summary>
		/// 其他设备
		/// <summary>
		public string EquipOther { get; set;}
		/// <summary>
		/// 用途
		/// <summary>
		public string Purpose { get; set;}
		/// <summary>
		/// 预订模式
		/// <summary>
		public string BookMode { get; set;}
		/// <summary>
		/// 管理单位
		/// <summary>
		public string ManagerUnit { get; set;}
		/// <summary>
		/// 管理人
		/// <summary>
		public string ManagerPerson { get; set;}
		/// <summary>
		/// 状态
		/// <summary>
		public string State { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 停用开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 停用结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 自定义字段
		/// <summary>
		public string Field00 { get; set;}
		public string Field01 { get; set;}
		public string Field02 { get; set;}

	}
}
