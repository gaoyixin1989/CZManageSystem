using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Domain
{
    /// <summary>
    /// 表单实例类.
    /// </summary>
    [Serializable]
    public class FormInstance : Botwave.Entities.TrackedEntity
    {
        private Guid id;
        private Guid formDefinitionId;
        private IList<FormItemInstance> items;

        /// <summary>
        /// 表单实例编号.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 表单定义Id.
        /// </summary>
        public Guid FormDefinitionId
        {
            get { return formDefinitionId; }
            set { formDefinitionId = value; }
        }

        /// <summary>
        /// 表单项.
        /// </summary>
        public IList<FormItemInstance> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
