using System;
using System.Collections.Generic;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// [xqp_WorkflowInMenuGroup] 的实体类.
    /// 创建日期: 2008-4-22
    /// </summary>
    public class WorkflowInMenuGroup
    {
        #region Getter / Setter

        private string workflowName;
        private int menuGroupId;
        private int showOrder;

        /// <summary>
        /// 流程名称
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// 分组ID
        /// </summary>
        public int MenuGroupId
        {
            get { return menuGroupId; }
            set { menuGroupId = value; }
        }

        /// <summary>
        /// 显示顺序 
        /// </summary>
        public int ShowOrder
        {
            get { return showOrder; }
            set { showOrder = value; }
        }

        #region 非持久化属性        
        private Guid workflowId;
        private string workflowAlias;
        private string aliasImage;

        /// <summary>
        /// 流程编号. 
        /// </summary>
        public Guid WorkflowId
        {
            get { return workflowId; }
            set { workflowId = value; }
        }

        /// <summary>
        /// 流程别名.
        /// </summary>
        public string WorkflowAlias
        {
            get { return workflowAlias; }
            set { workflowAlias = value; }
        }

        /// <summary>
        /// 流程别名图片.
        /// </summary>
        public string AliasImage
        {
            get { return aliasImage; }
            set { aliasImage = value; }
        }
        #endregion

        #endregion

        #region 数据操作

        public string Create()
        {
            IBatisMapper.Insert("xqp_WorkflowInMenuGroup_Insert", this);
            return this.workflowName;
        }

        public int Update()
        {
            return IBatisMapper.Update("xqp_WorkflowInMenuGroup_Update", this);
        }

        public int Delete()
        {
            return IBatisMapper.Delete("xqp_WorkflowInMenuGroup_Delete", this.WorkflowName);
        }

        public static bool IsExists(string workflowName)
        {
            return (IBatisMapper.Mapper.QueryForObject<int>("xqp_WorkflowInMenuGroup_Select_IsExists", workflowName) > 0);
        }

        public static WorkflowInMenuGroup LoadByName(string workflowName)
        {
            return IBatisMapper.Load<WorkflowInMenuGroup>("xqp_WorkflowInMenuGroup_Select_WorkflowName", workflowName);
        }

        public static IList<WorkflowInMenuGroup> Select()
        {
            return IBatisMapper.Select<WorkflowInMenuGroup>("xqp_WorkflowInMenuGroup_Select");
        }

        public static IList<WorkflowInMenuGroup> Select(string workflowName)
        {
            if (String.IsNullOrEmpty(workflowName))
            {
                return Select();
            }
            else
            {
                IList<WorkflowInMenuGroup> mgList = new List<WorkflowInMenuGroup>();
                WorkflowInMenuGroup mgEntity = null;
                string[] wfNameList = workflowName.Split('|');
                foreach (string name in wfNameList)
                {
                    mgEntity = IBatisMapper.Load<WorkflowInMenuGroup>("xqp_WorkflowInMenuGroup_Select_By_Workflow", name);
                    if (null != mgEntity)
                        mgList.Add(mgEntity);
                }
                return mgList;
            }
        }

        #endregion
    }
}

