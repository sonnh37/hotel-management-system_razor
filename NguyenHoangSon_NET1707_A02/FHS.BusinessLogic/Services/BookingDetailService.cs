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
    public class BookingDetailService
    {
        public readonly IBaseRepository<BookingDetail> _repository;
        private readonly IMapper _mapper;

        public BookingDetailService(IBaseRepository<BookingDetail> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //Create Method
        public async Task AddRangeBookingDetail(List<BookingDetail> bookingDetails)
        {
            try
            {
                if (bookingDetails == null)
                {
                    throw new ArgumentNullException(nameof(bookingDetails));
                }
                else
                {
                    await _repository.CreateRange(bookingDetails);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BookingDetail> GetBookingDetailByQueryable(Expression<Func<BookingDetail, bool>> predicate)
        {
            IQueryable<BookingDetail> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable
                    .Include(m => m.BookingReservation)
                    .Include(m => m.Room)
                    .SingleOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<BookingDetail>> GetBookingDetailListByQueryable(Expression<Func<BookingDetail, bool>> predicate)
        {
            IQueryable<BookingDetail> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable
                    .Include(m => m.BookingReservation)
                    .Include(m => m.Room)
                    .ToListAsync();
            }

            return null;
        }
    }
}
