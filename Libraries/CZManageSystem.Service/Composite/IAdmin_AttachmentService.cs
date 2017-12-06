using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
    public interface IAdmin_AttachmentService : IBaseService<Admin_Attachment>
    {
        IList<Admin_Attachment> GetAllAttachmentList(Guid id);
       
    }
}
