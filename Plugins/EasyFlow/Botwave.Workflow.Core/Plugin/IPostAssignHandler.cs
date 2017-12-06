using System;
using System.Collections.Generic;
using System.Text;

using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// ����ָ��֮��Ĵ�����.
    /// </summary>
    public interface IPostAssignHandler
    {
        /// <summary>
        /// ��һ�����ĺ���������.
        /// </summary>
        IPostAssignHandler Next { get; set; }

        /// <summary>
        /// ִ��.
        /// </summary>
        /// <param name="assignment"></param>
        void Execute(Assignment assignment);
    }
}
