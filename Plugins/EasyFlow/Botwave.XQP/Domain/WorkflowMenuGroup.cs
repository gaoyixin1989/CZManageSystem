using System;
using System.Collections.Generic;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
	/// <summary>
	/// [xqp_WorkflowMenuGroup] 的实体类.
	/// 创建日期: 2008-4-22
	/// </summary>
	public class WorkflowMenuGroup
	{
		#region Getter / Setter
		
		private int groupID;
		private string groupName = String.Empty;
		private int showOrder;
		
		/// <summary>
        /// 分组ID
        /// </summary>
		public int GroupID
		{
			get{ return groupID; }
			set{ groupID = value; }
		}

		/// <summary>
        /// 分组名称
        /// </summary>
		public string GroupName
		{
			get{ return groupName; }
			set{ groupName = value; }
		}

		/// <summary>
        /// 显示顺序
        /// </summary>
		public int ShowOrder
		{
			get{ return showOrder; }
			set{ showOrder = value; }
		}
		
		#endregion		
		
        #region 数据操作
		
        public int Create()
        {
            IBatisMapper.Insert("xqp_WorkflowMenuGroup_Insert", this);
            return this.GroupID;
        }

        public int Update()
        {
            return IBatisMapper.Update("xqp_WorkflowMenuGroup_Update", this);
        }

        public int Delete()
        {
            return IBatisMapper.Delete("xqp_WorkflowMenuGroup_Delete", this.GroupID);
        }

        public static WorkflowMenuGroup LoadById(int groupID)
        {
            return IBatisMapper.Load<WorkflowMenuGroup>("xqp_WorkflowMenuGroup_Select", groupID);
        }

        public static IList<WorkflowMenuGroup> Select()
        {
            return IBatisMapper.Select<WorkflowMenuGroup>("xqp_WorkflowMenuGroup_Select");
        }

        public static bool IsExists(string groupName)
        {
            return (IBatisMapper.Mapper.QueryForObject<int>("xqp_WorkflowMenuGroup_Select_By_Name", groupName) > 0);
        }

        #endregion
	}
}
	
