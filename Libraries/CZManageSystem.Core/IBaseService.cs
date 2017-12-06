using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CZManageSystem.Core;
using System.Data.Entity.Infrastructure;

namespace ZManageSystem.Core
{
    public interface IBaseService<T> where T : class
    {
        IQueryable<T> Entitys { get; }
        bool Contains(Expression<Func<T, bool>> whereCondition);
        Task<bool> ContainsAsync(Expression<Func<T, bool>> whereCondition);
        int Count(Expression<Func<T, bool>> exp);
        bool Delete(T entity);
        Task<bool> DeleteAsync(T entity);
        bool DeleteByList(List<T> entitys);
        Task<bool> DeleteByListAsync(List<T> entitys);
        Expression<Func<T, bool>> ExpressionFactory(object obj);
        T FindByFeldName(Expression<Func<T, bool>> expfeldName);
        Task<T> FindByFeldNameAsync(Expression<Func<T, bool>> expfeldName);
        T FindById(object Id);
        List<T> FindByKeyValues(params object[] keyValues);
        IEnumerable<dynamic> GetForPaging(out int total, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        bool Insert(T entity);
        Task<bool> InsertAsync(T entity);
        bool InsertByList(List<T> entitys);
        Task<bool> InsertByListAsync(List<T> entitys);
        IQueryable<T> List();
        Task<List<T>> ListAsync();
        bool Update(T entity);
        Task<bool> UpdateAsync(T entity);
        bool UpdateByList(List<T> entitys);
        Task<bool> UpdateByListAsync(List<T> entitys);
        DbRawSqlQuery<TKey> Execute<TKey>(string sqlQuery, params object[] parameters) where TKey : IComparable<TKey>;

        /// <summary>
        /// 执行存储过程返回实体对象
        /// </summary>
        /// <typeparam name="TKey">实体对象</typeparam>
        /// <param name="sqlQuery">存储过程</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        DbRawSqlQuery<TKey> ExecuteResT<TKey>(string sqlQuery, params object[] parameters) where TKey : class;
    }
}