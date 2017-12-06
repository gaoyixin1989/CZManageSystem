using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Composite
{
    public class Admin_Attachment
    {
        public Guid Id { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string FileName { get; set; }
        public string Fileupload { get; set; }
        public string MimeType { get; set; }
        public string FileSize { get; set; }
        public Nullable<Guid> Upguid { get; set; }



    }

}
