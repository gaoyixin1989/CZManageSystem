using System;
using System.Collections.Generic;

using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
	/// <summary>
	/// [xqp_WorkflowBusiness] 的实体类.
	/// 创建日期: 2008-7-21
	/// </summary>
	public class WorkflowBusiness
	{
		#region Getter / Setter
		
		private int id;
		private string workflowName = String.Empty;
		private string activityName = String.Empty;
		private string fieldName = String.Empty;
		private string fieldValue = String.Empty;
		private string fieldName2 = String.Empty;
		private string fieldValue2 = String.Empty;
		private string actor = String.Empty;
		
		/// <summary>
        /// 
        /// </summary>
		public int Id
		{
			get{ return id; }
			set{ id = value; }
		}

		/// <summary>
        /// 流程名称
        /// </summary>
		public string WorkflowName
		{
			get{ return workflowName; }
			set{ workflowName = value; }
		}

		/// <summary>
        /// 流程步骤
        /// </summary>
		public string ActivityName
		{
			get{ return activityName; }
			set{ activityName = value; }
		}

		/// <summary>
        /// 字段名称
        /// </summary>
		public string FieldName
		{
			get{ return fieldName; }
			set{ fieldName = value; }
		}

		/// <summary>
        /// 字段值
        /// </summary>
		public string FieldValue
		{
			get{ return fieldValue; }
			set{ fieldValue = value; }
		}

		/// <summary>
        /// 字段名称
        /// </summary>
		public string FieldName2
		{
			get{ return fieldName2; }
			set{ fieldName2 = value; }
		}

		/// <summary>
        /// 字段值
        /// </summary>
		public string FieldValue2
		{
			get{ return fieldValue2; }
			set{ fieldValue2 = value; }
		}

		/// <summary>
        /// 操作人
        /// </summary>
		public string Actor
		{
			get{ return actor; }
			set{ actor = value; }
		}
		
		#endregion		
		
        #region 数据操作
		
        public int Create()
        {
            IBatisMapper.Insert("xqp_WorkflowBusiness_Insert", this);
            return this.Id;
        }

        public int Update()
        {
            return IBatisMapper.Update("xqp_WorkflowBusiness_Update", this);
        }

        public int Delete()
        {
            return IBatisMapper.Delete("xqp_WorkflowBusiness_Delete", this.Id);
        }

        public static IList<WorkflowBusiness> SelectByWorkflow(string workflowName)
        {
            return IBatisMapper.Select<WorkflowBusiness>("xqp_WorkflowBusiness_Select_By_WorkflowName");
        }

        #endregion
	}
}
	
