using AutoMapper;
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
    public class BookingReservationService
    {
        public readonly IBaseRepository<BookingReservation> _repository;
        private readonly IMapper _mapper;

        public BookingReservationService(IBaseRepository<BookingReservation> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //Create Method
        public async Task<BookingReservation> AddBookingReservation(BookingReservation bookingReservation)
        {
            try
            {
                if (bookingReservation == null)
                {
                    throw new ArgumentNullException(nameof(bookingReservation));
                }
                else
                {
                    var cus = await GetBookingReservationByQueryable(m => m.BookingReservationId == bookingReservation.BookingReservationId);
                    if (cus != null)
                    {
                        return null;
                    }

                    return await _repository.Create(bookingReservation);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBookingReservation(int id)
        {
            try
            {
                var bookingReservation = await GetBookingReservationByQueryable(m => m.BookingReservationId == id);
                if (bookingReservation == null)
                {
                    throw new ArgumentNullException(nameof(bookingReservation));
                }

                bookingReservation.BookingStatus = Convert.ToByte(2);
                _repository.Update(bookingReservation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateBookingReservation(BookingReservation bookingReservation)
        {
            try
            {
                if (bookingReservation == null)
                {
                    throw new ArgumentNullException(nameof(bookingReservation));
                }
                var entity = await GetBookingReservationByQueryable(m => m.BookingReservationId == bookingReservation.BookingReservationId);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(bookingReservation));
                }
                entity = _mapper.Map(bookingReservation, entity);
                _repository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<BookingReservation>> GetAllBookingReservation()
        {
            try
            {
                IQueryable<BookingReservation> queryable = _repository.GetQueryable(m => m.BookingStatus != Convert.ToByte(2));
                if (queryable.Any())
                {
                    return await queryable
                        .Include(m => m.BookingDetails)
                        .Include(m => m.Customer)
                        .ToListAsync();
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BookingReservation> GetBookingReservationByQueryable(Expression<Func<BookingReservation, bool>> predicate)
        {
            IQueryable<BookingReservation> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable
                    .Include(m => m.BookingDetails)
                    .Include(m => m.Customer)
                    .SingleOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<BookingReservation>> GetBookingReservationListByQueryable(Expression<Func<BookingReservation, bool>> predicate)
        {
            IQueryable<BookingReservation> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable
                    .Include(m => m.BookingDetails)
                    .Include(m => m.Customer)
                    .ToListAsync();
            }

            return null;
        }
    }
}
