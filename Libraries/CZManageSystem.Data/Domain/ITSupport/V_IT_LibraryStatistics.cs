using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class V_IT_LibraryStatistics
	{
		public int ID { get; set;}
		public string Title { get; set;}
		public string Code { get; set;}
		public Nullable<DateTime> CreateTime { get; set;}
		public Nullable<DateTime> InputTime { get; set;}
		public Nullable<Guid> Operator { get; set;}
		public string Remark { get; set;}
		public Nullable<int> State { get; set;}
		public Nullable<Guid> SumbitUser { get; set;}
		public Nullable<int> Amount { get; set;}
		public string Type { get; set;}
		public string Model { get; set;}
		public string Name { get; set;}
		public string Trademark { get; set;}

	}
}
