using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 投资项目信息
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //投资项目查询条件
    public class InvestProjectQueryBuilder
    {
        public int? Year { get; set; }
        public string ProjectID { get; set; }//项目编号
        public string ProjectName { get; set; }//项目名称
        public string DpCode_Text { get; set; }//负责部门
        public string ManagerID_Text { get; set; }//项目经理
    }

    public class InvestProject
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 项目编号（唯一）
        /// <summary>
        public string ProjectID { get; set;}
		/// <summary>
		/// 下达年份,也是导入时间的年份
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// 计划任务书文号
		/// <summary>
		public string TaskID { get; set;}
		/// <summary>
		/// 项目名称
		/// <summary>
		public string ProjectName { get; set;}
		/// <summary>
		/// 起止年限
		/// <summary>
		public string BeginEnd { get; set;}
		/// <summary>
		/// 项目总投资
		/// <summary>
		public Nullable<decimal> Total { get; set;}
		/// <summary>
		/// 年度项目投资
		/// <summary>
		public Nullable<decimal> YearTotal { get; set;}
		/// <summary>
		/// 年度建设内容
		/// <summary>
		public string Content { get; set;}
		/// <summary>
		/// 要求完成时限
		/// <summary>
		public string FinishDate { get; set;}
		/// <summary>
		/// 负责专业室
		/// <summary>
		public string DpCode { get; set;}
		/// <summary>
		/// 室负责人
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// 项目经理
		/// <summary>
		public Nullable<Guid> ManagerID { get; set;}


        //外键
        //public virtual Depts DeptObj { get; set; }
        //public virtual Users UserObj { get; set; }
        //public virtual Users ManagerObj { get; set; }

    }
}
