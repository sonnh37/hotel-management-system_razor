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
    public class DeleteModel : PageModel
    {
        private readonly NguyenHoangSon_NET1707_A02.Data.FuminiHotelManagementContext _context;

        public DeleteModel(NguyenHoangSon_NET1707_A02.Data.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingreservation = await _context.BookingReservations.Include(m => m.Customer).FirstOrDefaultAsync(m => m.BookingReservationId == id);

            if (bookingreservation == null)
            {
                return NotFound();
            }
            else
            {
                BookingReservation = bookingreservation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingreservation = await _context.BookingReservations.FindAsync(id);
            if (bookingreservation != null)
            {
                BookingReservation = bookingreservation;
                BookingReservation.BookingStatus = Convert.ToByte(2);
                _context.BookingReservations.Update(BookingReservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
