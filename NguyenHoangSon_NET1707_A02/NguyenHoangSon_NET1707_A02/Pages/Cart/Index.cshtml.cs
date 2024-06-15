using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;

        public IndexModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IList<RoomInformation> RoomInformation { get; set; } = new List<RoomInformation>();

        [BindProperty]
        public DateView DateView { get; set; } = default!;

        public void OnGet()
        {
            if (Session.carts != null && Session.carts.Count > 0)
            {
                RoomInformation = Session.carts;
            }
            if (DateView == null)
            {
                DateView = new DateView();
                //DateView = new DateView
                //{
                //    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                //    EndDate = DateOnly.FromDateTime(DateTime.UtcNow),
                //};
            }
        }

        public IActionResult OnPostCheckout()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Pls select Start and End Date";
                OnGet();
                return Page();
            }

            if(!CheckDate(DateView.StartDate.Value, DateView.EndDate.Value))
            {
                TempData["Message"] = "End date must be greater than start date.";
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
                    _context.BookingReservations.Add(bookingReservation);
                    _context.BookingDetails.AddRange(bookingDetails);

                    TempData["Message"] = "Booking successfully added!";
                }
                else
                {
                    TempData["Message"] = "Pls select Start and End Date";
                }
            }
            else
            {
                TempData["Message"] = "Pls add more room";
            }

            // Clear the cart after successful checkout
            Session.carts.Clear();
            _context.SaveChanges();

            // Redirect to a confirmation page or home page
            return Redirect("/Cart");
        }

        private BookingReservation GetBookingReservation()
        {
            var email = HttpContext.Session.GetString("Username").ToLower().ToString();
            Customer customer = _context.Customers.Where(m => m.EmailAddress.ToLower() == email).SingleOrDefault();
            return new BookingReservation
            {
                BookingReservationId = _context.BookingReservations.Count(),
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

            int numberOfDays = endDate.DayNumber - startDate.DayNumber;

            return priceOrigin.Value * numberOfDays;
        }

        private bool CheckDate(DateOnly startDate, DateOnly endDate)
        {
            int numberOfDays = endDate.DayNumber - startDate.DayNumber;
            if (numberOfDays <= 0)
            {
                
                return false;
            }

            return true;
        }

    }
}
