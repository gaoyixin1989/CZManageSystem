using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    public class CommentResult:ApiResult
    {
        public CommentList[] CommentLists { get; set; }
    }

    public class CommentList
    {
        public string Creator { get; set; }

        public string Message { get; set; }

        public Attachment[] Attachments { get; set; }

        public string CreatedTime { get; set; }
    }

}
