using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow
{
    /// <summary>
    /// ����״̬����.
    /// </summary>
    public static class WorkflowConstants
    {
        /// <summary>
        /// ��ʼ״̬.
        /// </summary>
        public static readonly int Initial = 0;
        
        /// <summary>
        /// ����������.
        /// </summary>
        public static readonly int Executing = 1;

        /// <summary>
        /// ���������.
        /// </summary>
        public static readonly int Complete = 2;
        
        /// <summary>
        /// ������ȡ��.
        /// </summary>
        public static readonly int Cancel = 99;
    }

}
