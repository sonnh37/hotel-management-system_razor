using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class EditModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly BookingReservationService _bookingReservationService;
        private readonly IMapper _mapper;

        public EditModel(CustomerService customerService, BookingReservationService bookingReservationService, IMapper mapper)
        {
            _customerService = customerService;
            _bookingReservationService = bookingReservationService;
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

            var bookingreservation = await _bookingReservationService.GetBookingReservationByQueryable(m => m.BookingReservationId == id);
            if (bookingreservation == null)
            {
                return NotFound();
            }
            BookingReservation = _mapper.Map<BookingReservationView>(bookingreservation);
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomer(), "CustomerId", "CustomerFullName");
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

            await _bookingReservationService.UpdateBookingReservation(_mapper.Map<BookingReservation>(BookingReservation));

            return RedirectToPage("./Index");
        }

    }
}
