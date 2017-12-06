using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Domain
{
    /// <summary>
    /// 表单定义类.
    /// </summary>
    [Serializable]
    public class FormDefinition : Botwave.Entities.TrackedEntity
    {
        private Guid id;
        private string name;
        private int version;
        private bool isCurrentVersion;
        private bool enabled;
        private string comment;
        private string templateContent;
        private IList<FormItemDefinition> items;

        /// <summary>
        /// 表单定义 id.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 表单定义的名称.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 表单定义的版本.
        /// </summary>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 表单定义是否当前版本.
        /// </summary>
        public bool IsCurrentVersion
        {
            get { return isCurrentVersion; }
            set { isCurrentVersion = value; }
        }

        /// <summary>
        /// 表单定义是否有效.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 表单定义的注释.
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// 表单定义模板内容.
        /// </summary>
        public string TemplateContent
        {
            get { return templateContent; }
            set { templateContent = value; }
        }

        /// <summary>
        /// 表单项定义列表.
        /// </summary>
        public IList<FormItemDefinition> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
