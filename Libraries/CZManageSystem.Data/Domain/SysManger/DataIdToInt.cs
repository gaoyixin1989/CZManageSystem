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
        /// int���
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// ������Դ
        /// <summary>
        public string DataSource { get; set; }
        /// <summary>
        /// ����ID
        /// <summary>
        public string DataId { get; set; }

        public class DataSourceType
        {
            public static string ToDo = "����";
            public static string ToRead = "����";
            //public static string HasDo = "�Ѱ�";
            //public static string HasRead = "����";
        }


    }


}
