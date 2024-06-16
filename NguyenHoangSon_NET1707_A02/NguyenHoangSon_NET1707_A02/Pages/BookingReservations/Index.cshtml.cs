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

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("Role") != null)
            {
                BookingReservation = await _bookingReservationService.GetAllBookingReservation();
            }
            else
            {
                var email = HttpContext.Session.GetString("Username").ToLower().ToString();
                Customer customer = await _customerService.GetCustomerByQueryable(m => m.EmailAddress == email);
                BookingReservation = await _bookingReservationService.GetBookingReservationListByQueryable(m => m.CustomerId == customer.CustomerId && m.BookingStatus == Convert.ToByte(1));
                if (BookingReservation == null)
                {
                    BookingReservation = new List<BookingReservation>();
                }
            }

        }
    }
}
