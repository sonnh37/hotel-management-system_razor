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
    public class CustomerService
    {
        public readonly IBaseRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomerService(IBaseRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //Create Method
        public async Task<Customer> AddCustomer(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException(nameof(customer));
                }
                else
                {
                    var cus = await GetCustomerByQueryable(m => m.EmailAddress == customer.EmailAddress);
                    if (cus != null)
                    {
                        return null;
                    }

                    return await _repository.Create(customer);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCustomer(int id)
        {
            try
            {
                var customer = await GetCustomerByQueryable(m => m.CustomerId == id);
                if (customer == null)
                {
                    throw new ArgumentNullException(nameof(customer));
                }

                customer.CustomerStatus = Convert.ToByte(2);
                _repository.Update(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException(nameof(customer));
                }
                var entity = await GetCustomerByQueryable(m => m.CustomerId == customer.CustomerId);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(customer));
                }
                entity = _mapper.Map(customer, entity);
                _repository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Customer>> GetAllCustomer()
        {
            try
            {
                IQueryable<Customer> queryable = _repository.GetQueryable(m => m.CustomerStatus == Convert.ToByte(1));
                if (queryable.Any())
                {
                    return await queryable
                        .Include(m => m.BookingReservations)
                        .ToListAsync();
                }

                return [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(IList<Customer>, int)> GetAllCustomer(int pageNumber, int pageSize)
        {
            try
            {
                IQueryable<Customer> queryable = _repository.GetQueryable(m => m.CustomerStatus == Convert.ToByte(1));
                if (queryable.Any())
                {
                    queryable = queryable.Include(m => m.BookingReservations).OrderBy(m => m.CustomerFullName);
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

        public async Task<Customer> GetCustomerByQueryable(Expression<Func<Customer, bool>> predicate)
        {
            IQueryable<Customer> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable.SingleOrDefaultAsync();
            }

            return null;
        }

        public async Task<List<Customer>> GetCustomerListByQueryable(Expression<Func<Customer, bool>> predicate)
        {
            IQueryable<Customer> queryable = _repository.GetQueryable(predicate);
            if (queryable.Any())
            {
                return await queryable.ToListAsync();
            }

            return null;
        }
    }
}
