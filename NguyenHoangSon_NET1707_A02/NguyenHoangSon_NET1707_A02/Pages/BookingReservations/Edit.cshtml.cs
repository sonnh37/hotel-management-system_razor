using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class EditModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly IMapper _mapper;

        public EditModel(FuminiHotelManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public BookingReservationView BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingreservation =  await _context.BookingReservations.FirstOrDefaultAsync(m => m.BookingReservationId == id);
            if (bookingreservation == null)
            {
                return NotFound();
            }
            BookingReservation = _mapper.Map<BookingReservationView>(bookingreservation);
           ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerFullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(_mapper.Map<BookingReservation>(BookingReservation)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingReservationExists(BookingReservation.BookingReservationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BookingReservationExists(int id)
        {
            return _context.BookingReservations.Any(e => e.BookingReservationId == id);
        }
    }
}
