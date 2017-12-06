using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.DynamicForm.Extension.Domain
{
    /// <summary>
    /// 应用系统(Iframe)接入实体类
    /// </summary>
    public class FormItemIFrames
    {
        #region Getter / Setter

        private int appId;
        private Guid formItemDefinitionId;
        private string activityName = String.Empty;
        private string remark = String.Empty;
        private int settingType;
        private int enabled;
        private string accessUrl;
        private int height;
        private int width;
        private string creator = String.Empty;
        private string lastModifier = String.Empty;
        private DateTime createdTime;
        private DateTime lastModTime;

        /// <summary>
        /// ID.
        /// </summary>
        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        public Guid FormItemDefinitionId
        {
            get { return formItemDefinitionId; }
            set { formItemDefinitionId = value; }
        }

        /// <summary>
        /// 流程步骤名称.
        /// </summary>

        public string ActivityName
        {
            get { return activityName; }
            set { activityName = value; }
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
        /// 访问类型.
        /// 0 流程；1 步骤
        /// </summary>
        public int SettingType
        {
            get { return settingType; }
            set { settingType = value; }
        }

        /// <summary>
        /// 是否启用.
        /// 0 禁用；1 启用
        /// </summary>
        public int Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 接入地址.
        /// </summary>
        public string AccessUrl
        {
            get { return accessUrl; }
            set { accessUrl = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 最后修改人.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 最后修改时间.
        /// </summary>
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        #endregion
    }
}
