using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class IndexModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly CustomerService _customerService;
        private readonly BookingReservationService _bookingReservationService;

        public IndexModel(FuminiHotelManagementContext context, CustomerService customerService, BookingReservationService bookingReservationService)
        {
            _context = context;
            _customerService = customerService;
            _bookingReservationService = bookingReservationService;
        }

        public IList<BookingReservation> BookingReservation { get; set; } = default!;

        [BindProperty]
        public int totalPages { get; set; } = 1;

        [BindProperty]
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 5;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            if (HttpContext.Session.GetString("Role") != null)
            {
                this.pageNumber = pageNumber;
                await GetAll(pageNumber);
            }
            else
            {
                var email = HttpContext.Session.GetString("Username").ToLower().ToString();
                Customer customer = await _customerService.GetCustomerByQueryable(m => m.EmailAddress == email);

                this.pageNumber = pageNumber;
                var item = await _bookingReservationService.GetBookingReservationListByQueryable(m => m.CustomerId == customer.CustomerId && m.BookingStatus == Convert.ToByte(1), pageNumber, pageSize);
                BookingReservation = item.Item1;
                totalPages = item.Item2;
            }

        }

        public async Task GetAll(int pageNumber)
        {
            var item = await _bookingReservationService.GetAllBookingReservation(pageNumber, this.pageSize);
            BookingReservation = item.Item1;
            totalPages = item.Item2;
        }
    }
}
