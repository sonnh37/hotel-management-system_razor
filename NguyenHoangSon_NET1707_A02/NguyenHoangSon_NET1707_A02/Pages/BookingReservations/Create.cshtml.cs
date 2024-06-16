using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class CreateModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly BookingReservationService _bookingReservationService;
        private readonly IMapper _mapper;

        public CreateModel(CustomerService customerService, BookingReservationService bookingReservationService, IMapper mapper)
        {
            _customerService = customerService;
            _bookingReservationService = bookingReservationService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllCustomer(), "CustomerId", "CustomerFullName");
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

            BookingReservation.BookingReservationId = _bookingReservationService.GetAllBookingReservation().Result.Count() + 1;

            var booking = await _bookingReservationService.AddBookingReservation(_mapper.Map<BookingReservation>(BookingReservation));
            if (booking == null)
            {
                ModelState.AddModelError("", "Error while adding booking");
                return Page();
            }
            TempData["Message"] = "Add Succesfully.";
            return RedirectToPage("./Index");
        }
    }
}
