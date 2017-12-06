using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Domain
{
    /// <summary>
    /// 表单定义与外部实体的关系类.
    /// </summary>
    [Serializable]
    public class FormDefinitionsInExternals
    {
        private Guid formDefinitionId;
        private string entityType;
        private Guid entityId;

        /// <summary>
        /// 表单定义编号.
        /// </summary>
        public Guid FormDefinitionId
        {
            get { return formDefinitionId; }
            set { formDefinitionId = value; }
        }

        /// <summary>
        /// 外部实体类型.
        /// </summary>
        public string EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }

        /// <summary>
        /// 外部实体编号.
        /// </summary>
        public Guid EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }
    }
}
