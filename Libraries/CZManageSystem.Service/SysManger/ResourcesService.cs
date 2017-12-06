using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data;
using CZManageSystem.Core;


namespace CZManageSystem.Service.SysManger
{
    public class ResourcesService : BaseService<Resources>, IResourcesService
    {
        private readonly IRepository<Resources> _SysResourcesRepository;
        #region 实例化

        public ResourcesService()
        {
            this._SysResourcesRepository = new EfRepository<Resources>();
        }
        #endregion
        public string GetResourcesMaxId()
        {

            //var length = _entityStore.Table.Where(x => x.ResourceId.StartsWith("CD")).Max(x => x.ResourceId.Length);
            //var v = _entityStore.Table.Where(x => x.ResourceId.StartsWith("CD") && x.ResourceId.Length == length).Max(x => x.ResourceId);
            //v = v ?? "CD0";
            //int index = int.Parse(v.Replace("CD", "")) + 1;

            var v = _entityStore.Table.Where(x => x.ResourceId.StartsWith("CD")).Select(x => x.ResourceId.Replace("CD", "")).ToList().ConvertAll(i => int.Parse(i));
            int index = v.Count() > 0 ? v.Max()+1 : 1;
            return "CD" + index;
          
        }
        public IList<Resources> GetResourcesByPid(string Pid)
        {
            return 
              this._SysResourcesRepository.Table.Where(t => t.ParentId == Pid).OrderBy(t => t.SortIndex).ToList<Resources>();
        }
    }

}
