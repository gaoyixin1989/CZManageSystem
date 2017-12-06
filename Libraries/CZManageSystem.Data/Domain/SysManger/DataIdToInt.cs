using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class DataIdToInt
    {
        /// <summary>
        /// int编号
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// 数据来源
        /// <summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 数据ID
        /// <summary>
        public string DataId { get; set; }

        public class DataSourceType
        {
            public static string ToDo = "待办";
            public static string ToRead = "待阅";
            //public static string HasDo = "已办";
            //public static string HasRead = "已阅";
        }


    }


}
