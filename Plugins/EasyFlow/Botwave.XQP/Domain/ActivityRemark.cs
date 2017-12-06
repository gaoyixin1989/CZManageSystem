using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 步骤审批意见类.
    /// </summary>
    public class ActivityRemark
    {
        /// <summary>
        /// 审批意见缓存的属性键名.
        /// </summary>
        public const string WorkflowRemarkPropertyKey = "Property_ActvityRemarks";

        #region Get / Set

        private int _id;
        private string _workflowName;
        private string _activityName;
        private string _remarkText;
        private string _remarkValue;
        private DateTime _createdTime;
        private int _displayOrder;

        /// <summary>
        /// 意见编号.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// 步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        /// <summary>
        /// 意见显示名称（文本）.
        /// </summary>
        public string RemarkText
        {
            get { return _remarkText; }
            set { _remarkText = value; }
        }

        /// <summary>
        /// 意见内容（值）.
        /// </summary>
        public string RemarkValue
        {
            get { return _remarkValue; }
            set { _remarkValue = value; }
        }

        /// <summary>
        /// 意见创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        /// <summary>
        /// 排序序号.
        /// </summary>
        public int DisplayOrder
        {
            get { return _displayOrder; }
            set { _displayOrder = value; }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 插入步骤审批意见.
        /// </summary>
        public void Insert()
        {
            IBatisMapper.Insert("cz_ActivityRemark_Insert", this);
        }

        /// <summary>
        /// 更新步骤审批意见内容.
        /// </summary>
        /// <returns></returns>
        public int UpdateRemark()
        {
            return ActivityRemark.Update(this.Id, this.RemarkText, this.RemarkValue);
        }

        /// <summary>
        /// 更新指定意见编号的步骤审批意见内容.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remarkText"></param>
        /// <param name="remarkValue"></param>
        /// <returns></returns>
        public static int Update(int id, string remarkText, string remarkValue)
        {
            Hashtable parameters = new Hashtable(3);
            parameters.Add("Id", id);
            parameters.Add("RemarkText", remarkText);
            parameters.Add("RemarkValue", remarkValue);

            return IBatisMapper.Update("cz_ActivityRemark_Update_ById", parameters);
        }

        /// <summary>
        /// 删除审批意见.
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return ActivityRemark.Delete(this.Id);
        }

        /// <summary>
        /// 删除指定意见编号的审批意见.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(int id)
        {
            return IBatisMapper.Delete("cz_ActivityRemark_Delete", id);
        }

        /// <summary>
        /// 检查步骤审批意见是否存在.返回值 true 则表示已经存在.
        /// </summary>
        /// <returns></returns>
        public bool IsExists()
        {
            object result = IBatisMapper.Mapper.QueryForObject("cz_ActivityRemark_Select_IsExists", this);
            return (result != null);
        }

        /// <summary>
        /// 获取指定步骤编号的步骤审批意见列表.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static IList<ActivityRemark> SelectByActivityId(Guid activityId)
        {
            return IBatisMapper.Select<ActivityRemark>("cz_ActivityRemark_Select", activityId);
        }

        #endregion
    }
}
