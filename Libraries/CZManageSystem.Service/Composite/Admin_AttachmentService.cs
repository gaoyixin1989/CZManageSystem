using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Composite;
using System.Linq.Expressions;

namespace CZManageSystem.Service.Composite
{
    public partial class Admin_AttachmentService : BaseService<Admin_Attachment>, IAdmin_AttachmentService
    {
        public IList<Admin_Attachment> GetAllAttachmentList(Guid id)
        {
            var query = this._entityStore.Table.Where(a => a.Upguid == id);
            List<Admin_Attachment> List = new List<Admin_Attachment>(query);
            return List;
        }
    }
}
