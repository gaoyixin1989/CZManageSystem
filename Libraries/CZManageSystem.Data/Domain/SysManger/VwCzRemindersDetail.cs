using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class VwCzRemindersDetail
	{
		public int ID { get; set;}
		public string SystemID { get; set;}
		public string MsgRefID { get; set;}
		public Nullable<int> MsgType { get; set;}
		public string Sender { get; set;}
		public string Receiver { get; set;}
		public string Subject { get; set;}
		public string Content { get; set;}
		public Nullable<int> State { get; set;}
		public Nullable<DateTime> CreatedTime { get; set;}
		public Nullable<DateTime> ProcessedTime { get; set;}
		public Nullable<int> RetriedTimes { get; set;}
		public string EntityType { get; set;}
		public string EntityId { get; set;}
		public string SenderEmployeeId { get; set;}
		public string ReceiverEmployeeId { get; set;}
		public string ReceiverEmail { get; set;}
		public string ReceiverMobile { get; set;}

	}
}
