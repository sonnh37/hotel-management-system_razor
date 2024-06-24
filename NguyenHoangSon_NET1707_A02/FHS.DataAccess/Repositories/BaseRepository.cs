using FHS.DataAccess.Contracts;
using FHS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FHS.DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly FuminiHotelManagementContext _context;

        public BaseRepository(FuminiHotelManagementContext dbContext)
        {
            _context = dbContext;
        }

        #region DbSet
        protected DbSet<TEntity> DbSet
        {
            get
            {
                var dbSet = GetDbSet<TEntity>();
                return dbSet;
            }
        }

        protected DbSet<T> GetDbSet<T>() where T : class
        {
            var dbSet = _context.Set<T>();
            return dbSet;
        }
        #endregion

        #region GetQueryable(CancellationToken) + GetQueryable() + GetQueryable(Expression<Func<TEntity, bool>>)

        public IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = GetQueryable<TEntity>();
            return queryable;
        }

        public IQueryable<T> GetQueryable<T>()
            where T : class
        {
            IQueryable<T> queryable = GetDbSet<T>();
            return queryable;
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> queryable = GetQueryable<TEntity>();
            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }
            return queryable;
        }

        public async Task<TEntity> Create(TEntity _object)
        {
            try
            {
                if (_object != null)
                {
                    var obj = DbSet.Add(_object);
                    await _context.SaveChangesAsync();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateRange(List<TEntity> _objects)
        {
            try
            {
                if (_objects != null)
                {
                    DbSet.AddRange(_objects);
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(TEntity _object)
        {
            try
            {
                if (_object != null)
                {
                    var obj = DbSet.Remove(_object);
                    if (obj != null)
                    {
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(TEntity _object)
        {
            try
            {
                if (_object != null)
                {
                    _context.Attach(_object).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IList<TEntity>> GetAll()
        {
            try
            {
                var obj = await DbSet.ToListAsync();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        #endregion
    }
}
