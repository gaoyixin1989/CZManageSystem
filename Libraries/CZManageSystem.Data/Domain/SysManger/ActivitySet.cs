using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ActivitySet
	{
		public Guid SetId { get; set;}
		public Guid ActivityId { get; set;}


        //public virtual Activities Activities { get; set; }

    }
}
