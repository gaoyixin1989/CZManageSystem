using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    ///自定义报表.
    /// </summary>
    public class WorkflowCustomReport
    {
        #region Getter / Setter

        private int id;
        private string workflowName = String.Empty;
        private string name = String.Empty;
        private string remark = String.Empty;
        private string content = String.Empty;
        private string showFields = String.Empty;
        private string conditions = String.Empty;
        private string creator = String.Empty;
        private DateTime createdTime;

        /// <summary>
        /// ID.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return workflowName; }
            set { workflowName = value; }
        }

        /// <summary>
        /// 报表名称.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 备注.
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 内容.
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 显示字段.
        /// </summary>
        public string ShowFields
        {
            get { return showFields; }
            set { showFields = value; }
        }

        /// <summary>
        /// 条件.
        /// </summary>
        public string Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }

        /// <summary>
        /// 创建者.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 创建日期.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        #endregion

        #region 数据操作

        /// <summary>
        /// 创建当前自定义报表.
        /// </summary>
        /// <returns></returns>
        public int Create()
        {
            IBatisMapper.Insert("xqp_CustomReport_Insert", this);
            return this.Id;
        }

        /// <summary>
        /// 更新当前自定义报表.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_CustomReport_Update", this);
        }

        /// <summary>
        /// 删除当前自定义报表.
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return IBatisMapper.Delete("xqp_CustomReport_Delete", this.Id);
        }

        /// <summary>
        /// 获取指定编号的自定义报表.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static WorkflowCustomReport LoadById(int id)
        {
            return IBatisMapper.Load<WorkflowCustomReport>("xqp_CustomReport_Select", id);
        }

        /// <summary>
        /// 获取全部自定义报表列表.
        /// </summary>
        /// <returns></returns>
        public static IList<WorkflowCustomReport> Select()
        {
            return IBatisMapper.Select<WorkflowCustomReport>("xqp_CustomReport_Select");
        }

        /// <summary>
        /// 获取指定用户的自定义报表列表.
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static IList<WorkflowCustomReport> Select(string actor)
        {
            return IBatisMapper.Select<WorkflowCustomReport>("xqp_CustomReport_Select_By_User", actor);
        }
        #endregion
    }
}
