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
    public class DetailsModel : PageModel
    {
        private readonly BookingReservationService _bookingReservationService;
        private readonly BookingDetailService _bookingDetailService;
        private readonly IMapper _mapper;

        public DetailsModel(BookingDetailService bookingDetailService, BookingReservationService bookingReservationService, IMapper mapper)
        {
            _bookingReservationService = bookingReservationService;
            _bookingDetailService = bookingDetailService;
            _mapper = mapper;
        }

        public BookingReservation BookingReservation { get; set; } = default!;

        public IList<BookingDetail> BookingDetail { get; set; } = default!;

        [BindProperty]
        public int totalPages { get; set; } = 1;

        [BindProperty]
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 5;

        public async Task<IActionResult> OnGetAsync(int? id, int pageNumber = 1)
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
                this.pageNumber = pageNumber;
                BookingReservation = bookingreservation;
                var item = await _bookingDetailService.GetBookingDetailListByQueryable(m => m.BookingReservationId == bookingreservation.BookingReservationId, this.pageNumber, pageSize);
                BookingDetail = item.Item1;
                totalPages = item.Item2;
                if (BookingDetail == null)
                {
                    BookingDetail = new List<BookingDetail>();
                }
            }
            return Page();
        }
    }
}
