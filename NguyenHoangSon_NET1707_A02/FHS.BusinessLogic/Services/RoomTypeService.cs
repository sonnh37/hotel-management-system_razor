using AutoMapper;
using FHS.DataAccess.Contracts;
using FHS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FHS.BusinessLogic.Services
{
    public class RoomTypeService
    {
        public readonly IBaseRepository<RoomType> _repository;
        private readonly IMapper _mapper;

        public RoomTypeService(IBaseRepository<RoomType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IList<RoomType>> GetAllRoomType()
        {
            try
            {
                IQueryable<RoomType> queryable = _repository.GetQueryable(m => m.RoomTypeName != null);
                if (queryable.Any())
                {
                    return await queryable
                        .ToListAsync();
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoomType> GetRoomTypeByQueryable(Expression<Func<RoomType, bool>> predicate)
        {
            IQueryable<RoomType> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable
                    .SingleOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<RoomType>> GetRoomTypeListByQueryable(Expression<Func<RoomType, bool>> predicate)
        {
            IQueryable<RoomType> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable
                    .ToListAsync();
            }

            return null;
        }
    }
}
