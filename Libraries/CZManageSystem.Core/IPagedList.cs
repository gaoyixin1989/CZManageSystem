
using System.Collections.Generic;
using System.Linq;

namespace CZManageSystem.Core
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        IQueryable<T> QueryPagedList(IQueryable<T> source, int pageIndex, int pageSize, out int total);
    }
}
