﻿using AutoMapper;
using FHS.BusinessLogic.Contracts;
using FHS.BusinessLogic.Views;
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
    public class RoomInformationService
    {
        public readonly IBaseRepository<RoomInformation> _repository;
        private readonly IMapper _mapper;

        public RoomInformationService(IBaseRepository<RoomInformation> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //Create Method
        public async Task<RoomInformation> AddRoomInformation(RoomInformation roomInformation)
        {
            try
            {
                if (roomInformation == null)
                {
                    throw new ArgumentNullException(nameof(roomInformation));
                }
                else
                {
                    var cus = await GetRoomInformationByQueryable(m => m.RoomId == roomInformation.RoomId);
                    if (cus != null)
                    {
                        return null;
                    }

                    return await _repository.Create(roomInformation);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteRoomInformation(int id)
        {
            try
            {
                var roomInformation = await GetRoomInformationByQueryable(m => m.RoomId == id);
                if (roomInformation == null)
                {
                    throw new ArgumentNullException(nameof(roomInformation));
                }

                roomInformation.RoomStatus = Convert.ToByte(2);
                _repository.Update(roomInformation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateRoomInformation(RoomInformation roomInformation)
        {
            try
            {
                if (roomInformation == null)
                {
                    throw new ArgumentNullException(nameof(roomInformation));
                }
                var entity = await GetRoomInformationByQueryable(m => m.RoomId == roomInformation.RoomId);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(roomInformation));
                }
                entity = _mapper.Map(roomInformation, entity);
                _repository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(IList<RoomInformation>, int)> GetAllRoomInformation(int pageNumber, int pageSize)
        {
            try
            {
                IQueryable<RoomInformation> queryable = _repository.GetQueryable(m => m.RoomStatus == Convert.ToByte(1));

                if (queryable.Any())
                {
                    queryable = queryable.Include(m => m.RoomType).OrderBy(m => m.RoomNumber);
                }

                var totalRecords = queryable.Count();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                var list = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return (list, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoomInformation> GetRoomInformationByQueryable(Expression<Func<RoomInformation, bool>> predicate)
        {
            IQueryable<RoomInformation> queryable = _repository.GetQueryable(predicate);

            return await queryable
                    .Include(m => m.RoomType)
                    .SingleOrDefaultAsync();
        }

        public async Task<(List<RoomInformation>, int)> GetRoomInformationListByQueryable(Expression<Func<RoomInformation, bool>> predicate, int pageNumber, int pageSize)
        {
            IQueryable<RoomInformation> queryable = _repository.GetQueryable(predicate);

            if (queryable.Any())
            {
                queryable = queryable.Include(m => m.RoomType).OrderBy(m => m.RoomNumber);
            }

            var totalRecords = queryable.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            var list = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (list, totalPages);
        }
    }
}
