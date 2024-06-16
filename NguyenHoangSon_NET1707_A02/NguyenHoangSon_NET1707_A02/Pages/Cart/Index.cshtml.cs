using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;
using AutoMapper;

namespace NguyenHoangSon_NET1707_A02.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly BookingReservationService _bookingReservationService;
        private readonly BookingDetailService _bookingDetailService;
        private readonly IMapper _mapper;

        public IndexModel(CustomerService customerService, BookingReservationService bookingReservationService, IMapper mapper, BookingDetailService bookingDetailService)
        {
            _customerService = customerService;
            _bookingReservationService = bookingReservationService;
            _mapper = mapper;
            _bookingDetailService = bookingDetailService;
        }

        public IList<RoomInformation> RoomInformation { get; set; } = new List<RoomInformation>();

        [BindProperty]
        public DateView DateView { get; set; } = default!;

        [BindProperty]
        public long TotalPrice { get; set; } = default!;

        public void OnGet()
        {
            if (Session.carts != null && Session.carts.Count > 0)
            {
                RoomInformation = Session.carts;
            }
            if (DateView == null)
            {
                DateView = new DateView
                {
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    EndDate = DateOnly.FromDateTime(DateTime.UtcNow),
                };
            }
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Pls select Start and End Date.");
                OnGet();
                return Page();
            }

            if (!CheckDate(DateView.StartDate.Value, DateView.EndDate.Value))
            {
                ModelState.AddModelError("", "End date must be greater than start date.");
                OnGet();
                return Page();
            }

            if (Session.carts == null || Session.carts.Count == 0)
            {
                // Handle empty cart case
                ModelState.AddModelError(string.Empty, "Your cart is empty.");
                return Page();
            }

            BookingReservation bookingReservation = GetBookingReservation();

            List<BookingDetail> bookingDetails = new List<BookingDetail>();

            if (Session.carts.Count() > 0)
            {
                if (DateView.StartDate.HasValue && DateView.EndDate.HasValue)
                {
                    foreach (RoomInformation room in Session.carts)
                    {
                        BookingDetail bookingDetail = GetBookingDetail(bookingReservation.BookingReservationId
                            , room, DateView.StartDate.Value, DateView.EndDate.Value);
                        bookingDetails.Add(bookingDetail);
                        bookingReservation.TotalPrice += bookingDetail.ActualPrice;
                    }

                    //Add
                    await _bookingReservationService.AddBookingReservation(bookingReservation);
                    await _bookingDetailService.AddRangeBookingDetail(bookingDetails);

                    TempData["Message"] = "Booking successfully added!";
                }
                else
                {
                    ModelState.AddModelError("", "Pls select Start and End Date.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Pls add more room.");
            }

            // Clear the cart after successful checkout
            Session.carts.Clear();

            // Redirect to a confirmation page or home page
            return Redirect("/BookingReservations/Index");
        }

        private BookingReservation GetBookingReservation()
        {
            var email = HttpContext.Session.GetString("Username").ToLower().ToString();
            Customer customer = _customerService.GetCustomerByQueryable(m => m.EmailAddress == email).Result;
            return new BookingReservation
            {
                BookingReservationId = _bookingReservationService.GetAllBookingReservation().Result.Count() + 1,
                BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
                TotalPrice = 0,
                CustomerId = customer.CustomerId,
                BookingStatus = Convert.ToByte(1),
            };
        }

        private BookingDetail GetBookingDetail(int bookingReservationId, RoomInformation room, DateOnly startDate, DateOnly endDate)
        {
            return new BookingDetail
            {
                BookingReservationId = bookingReservationId,
                RoomId = room.RoomId,
                StartDate = startDate,
                EndDate = endDate,
                ActualPrice = GetPriceFromStartDateToEndDate(room.RoomPricePerDay, startDate, endDate),
            };
        }

        private decimal GetPriceFromStartDateToEndDate(decimal? priceOrigin, DateOnly startDate, DateOnly endDate)
        {
            if (!priceOrigin.HasValue)
            {
                TempData["Message"] = "Room price per day must be specified.";
            }

            int numberOfDays = endDate.DayNumber - startDate.DayNumber + 1;

            return priceOrigin.Value * numberOfDays;
        }

        private bool CheckDate(DateOnly startDate, DateOnly endDate)
        {
            int numberOfDays = endDate.DayNumber - startDate.DayNumber + 1;
            if (numberOfDays <= 0)
            {

                return false;
            }

            return true;
        }

    }
}
