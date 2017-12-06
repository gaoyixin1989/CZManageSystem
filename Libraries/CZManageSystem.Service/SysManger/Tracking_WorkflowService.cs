using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.SysManger
{
    public class Tracking_WorkflowService : BaseService<Tracking_Workflow>, ITracking_WorkflowService
    {
        public IList<string> GetCurrentActivityNames(Guid guid)
        {
            string sql = string.Format("select dbo.fn_bwwf_GetCurrentActivityNames('{0}')",guid.ToString());
            var list = this._entityStore.ExecuteResT<string>(sql).ToList();
            return list;
        }
    }
}
