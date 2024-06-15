using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class IndexModel : PageModel
    {
        private readonly NguyenHoangSon_NET1707_A02.Data.FuminiHotelManagementContext _context;

        public IndexModel(NguyenHoangSon_NET1707_A02.Data.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IList<BookingReservation> BookingReservation { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("Role") != null)
            {
                BookingReservation = await _context.BookingReservations.Where(m => m.BookingStatus == Convert.ToByte(1))
                .Include(m => m.Customer)
                .Include(m => m.BookingDetails).ToListAsync();
            }
            else
            {
                var email = HttpContext.Session.GetString("Username").ToLower().ToString();
                Customer customer = _context.Customers.Where(m => m.EmailAddress.ToLower() == email).SingleOrDefault();
                BookingReservation = await _context.BookingReservations.Where(m => m.CustomerId == customer.CustomerId && m.BookingStatus == Convert.ToByte(1))
                .Include(m => m.Customer)
                .Include(m => m.BookingDetails).ToListAsync();
            }

        }
    }
}
