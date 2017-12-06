using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
    public class ConsumableQueryBuilder
    {
        //public int ID { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }
        public string Model { get; set; }

        public string IsValue { get; set; }

        public string IsDelete { get; set; }

        public DateTime? ApplyTime_start { get; set; }
        public DateTime? ApplyTime_end { get; set; }

        public bool? hasStock { get; set; }
    }

    public class Consumable
	{
		public int ID { get; set;}
		/// <summary>
		/// 耗材类别
		/// <summary>
		public string Type { get; set;}
		/// <summary>
		/// 耗材型号
		/// <summary>
		public string Model { get; set;}
		/// <summary>
		/// 耗材名称
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// 适用设备
		/// <summary>
		public string Equipment { get; set;}
		/// <summary>
		/// 单位
		/// <summary>
		public string Unit { get; set;}
		/// <summary>
		/// 耗材品牌
		/// <summary>
		public string Trademark { get; set;}
        /// <summary>
        /// 价值类型，0低价值，1非低价值
        /// <summary>
        public string IsValue { get; set;}
        /// <summary>
        /// 耗材当前拥有量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否删除  0代表否  1代表是
        /// </summary>
        public string IsDelete { get; set; }


    }
}
