using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Proj
	{
		public int Id { get; set;}
		public string ProjSn { get; set;}
		public string  ProjName { get; set;}
        public string Editor { get; set; }
        public DateTime EditTime { get; set; }

    }
}
