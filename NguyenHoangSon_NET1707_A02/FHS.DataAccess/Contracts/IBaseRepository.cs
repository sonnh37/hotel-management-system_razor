using System;
using System.Linq.Expressions;
namespace FHS.DataAccess.Contracts
{
    public interface IBaseRepository<T>
    {
        public Task<T> Create(T _object);
        public void Delete(T _object);
        public void Update(T _object);
        public Task<IList<T>> GetAll();
        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate);
        public Task CreateRange(List<T> _objects);
    }
}

