using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// °à´ÎÖµ°à°²ÅÅ
    /// </summary>
	public class ShiftRich
	{
        /// <summary>
        /// ±àºÅ
        /// <summary>
        public Guid Id { get; set;}
		/// <summary>
		/// ±à¼­ÈË
		/// <summary>
		public Nullable<Guid> Editor { get; set;}
		/// <summary>
		/// ±à¼­Ê±¼ä
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// °à´Îid
		/// <summary>
		public Guid BanciId { get; set;}
		public string Day01 { get; set;}
		public string Day02 { get; set;}
		public string Day03 { get; set;}
		public string Day04 { get; set;}
		public string Day05 { get; set;}
		public string Day06 { get; set;}
		public string Day07 { get; set;}
		public string Day08 { get; set;}
		public string Day09 { get; set;}
		public string Day10 { get; set;}
		public string Day11 { get; set;}
		public string Day12 { get; set;}
		public string Day13 { get; set;}
		public string Day14 { get; set;}
		public string Day15 { get; set;}
		public string Day16 { get; set;}
		public string Day17 { get; set;}
		public string Day18 { get; set;}
		public string Day19 { get; set;}
		public string Day20 { get; set;}
		public string Day21 { get; set;}
		public string Day22 { get; set;}
		public string Day23 { get; set;}
		public string Day24 { get; set;}
		public string Day25 { get; set;}
		public string Day26 { get; set;}
		public string Day27 { get; set;}
		public string Day28 { get; set;}
		public string Day29 { get; set;}
		public string Day30 { get; set;}
		public string Day31 { get; set;}
		/// <summary>
		/// ÅÅĞò
		/// <summary>
		public Nullable<int> OrderNo { get; set;}
		/// <summary>
		/// ±¸×¢
		/// <summary>
		public string Remark { get; set;}
        

    }
}
