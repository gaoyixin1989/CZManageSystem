using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data.Domain.Composite;

namespace CZManageSystem.Data.Domain.HumanResources.Knowledge
{
	public class SysKnowledge
	{
		/// <summary>
		/// id
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Title { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Content { get; set;}
		public int? OrderNo { get; set;}
		public Nullable<DateTime> Createdtime { get; set;}
		public Guid? CreatorID { get; set;}

        //
        public virtual Users CreatorObj { get; set; }//������
        //public virtual ICollection<Admin_Attachment> Attachments { get; set; }//����

    }
}
