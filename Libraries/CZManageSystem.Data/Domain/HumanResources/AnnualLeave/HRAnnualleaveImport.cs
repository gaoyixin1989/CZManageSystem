using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.AnnualLeave
{
    public class HRAnnualleaveImport
    {
        public int Id { get; set; }
        public string Importor { get; set; }
        public Nullable<DateTime> ImportTime { get; set; }
        public string ImportSn { get; set; }
        public string ImportTitle { get; set; }
        public string ImportMsg { get; set; }
        public string ImportInformation { get; set; }
        public string Remark { get; set; }
        public string ImportType { get; set; }

    }

    public class HRAnnualleaveImportQueryBuilder
    {
        public string ImportType { get; set; }//地点
    }

}
