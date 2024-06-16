using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using AutoMapper;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.BookingReservations
{
    public class DeleteModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly BookingReservationService _bookingReservationService;
        private readonly IMapper _mapper;

        public DeleteModel(CustomerService customerService, BookingReservationService bookingReservationService, IMapper mapper)
        {
            _customerService = customerService;
            _bookingReservationService = bookingReservationService;
            _mapper = mapper;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

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

            await _bookingReservationService.DeleteBookingReservation(id.Value);
            TempData["Message"] = "Delete Succesfully.";
            return RedirectToPage("./Index");
        }
    }
}
