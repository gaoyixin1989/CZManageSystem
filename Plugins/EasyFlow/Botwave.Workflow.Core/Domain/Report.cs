using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ����ͳ����.
    /// </summary>
    public class Report
    {
        private string statName;
        private int statInstance;
        private Guid statid;

        /// <summary>
        /// ͳ������.
        /// </summary>
        public string StatName
        {
            get { return statName; }
            set { statName = value; }
        }

        /// <summary>
        /// ͳ��ʵ��.
        /// </summary>
        public int StatInstance
        {
            get { return statInstance; }
            set { statInstance = value; }
        }

        /// <summary>
        /// ͳ�Ʊ��.
        /// </summary>
        public Guid StatID
        {
            get { return statid; }
            set { statid = value; }
        }
    }
}
