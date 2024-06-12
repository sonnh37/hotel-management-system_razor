using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class CreateModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly IMapper _mapper;

        public CreateModel(FuminiHotelManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerFullName");
            return Page();
        }

        [BindProperty]
        public BookingReservationView BookingReservation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // count + 1

            BookingReservation.BookingReservationId = _context.BookingReservations.Count() + 1;

            _context.BookingReservations.Add(_mapper.Map<BookingReservation>(BookingReservation));
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
