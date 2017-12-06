using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Allocator
{
    /// <summary>
    /// 任务分派变量类.
    /// </summary>
    public class TaskVariable
    {
        private string id;
        private string actor;
        private object expression;
        private object args;
        private IDictionary<string, object> properties;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public TaskVariable()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="id">编号.</param>
        /// <param name="actor">操作人.</param>
        public TaskVariable(string id, string actor)
            : this(id, actor, null, null)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="id">编号.</param>
        /// <param name="actor">操作人.</param>
        /// <param name="expression">表达式对象.</param>
        /// <param name="args">参数对象.</param>
        public TaskVariable(string id, string actor, object expression, object args)
            : this(id, actor, expression, args, new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase))
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="id">编号.</param>
        /// <param name="actor">操作人.</param>
        /// <param name="expression">表达式对象.</param>
        /// <param name="args">参数对象.</param>
        /// <param name="variableProperties">变量属性字典.</param>
        public TaskVariable(string id, string actor, object expression, object args, IDictionary<string, object> variableProperties)
        {
            this.id = id;
            this.actor = actor;
            this.expression = expression;
            this.args = args;
            this.properties = variableProperties;
        }

        /// <summary>
        /// 编号.
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 操作人.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// 表达式对象.
        /// </summary>
        public object Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        /// <summary>
        /// 参数对象.
        /// </summary>
        public object Args
        {
            get { return args; }
            set { args = value; }
        }

        /// <summary>
        /// 变量属性字典.
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get { return properties; }
            set { properties = value; }
        }
    }
}
