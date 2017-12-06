using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 报表统计类.
    /// </summary>
    public class Report
    {
        private string statName;
        private int statInstance;
        private Guid statid;

        /// <summary>
        /// 统计名称.
        /// </summary>
        public string StatName
        {
            get { return statName; }
            set { statName = value; }
        }

        /// <summary>
        /// 统计实例.
        /// </summary>
        public int StatInstance
        {
            get { return statInstance; }
            set { statInstance = value; }
        }

        /// <summary>
        /// 统计编号.
        /// </summary>
        public Guid StatID
        {
            get { return statid; }
            set { statid = value; }
        }
    }
}
