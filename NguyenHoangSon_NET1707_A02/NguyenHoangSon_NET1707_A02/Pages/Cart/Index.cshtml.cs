using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;
using AutoMapper;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public IList<DateView> DateViews { get; set; } = default!;
        public static IList<DateView> CardDynamics { get; set; } = default!;

        [BindProperty]
        public decimal? TotalPrice { get; set; } = default!;

        public void OnGet()
        {
            if (DateViews == null)
            {
                DateViews = new List<DateView>();
            }

            if (Session.carts != null && Session.carts.Count > 0)
            {
                var dateNow = DateOnly.FromDateTime(DateTime.UtcNow);

                // Check if CardDynamics is already initialized and contains data
                if (CardDynamics != null && CardDynamics.Count > 0)
                {
                    DateViews = CardDynamics;
                    if (Session.carts.Count > CardDynamics.Count)
                    {
                        foreach (var cart in Session.carts)
                        {
                            if (!DateViews.Any(dv => dv.Room.RoomId == cart.RoomId))
                            {
                                DateViews.Add(new DateView
                                {
                                    Room = cart,
                                    StartDate = dateNow,
                                    EndDate = dateNow,
                                    ActualPrice = GetPriceFromStartDateToEndDate(cart.RoomPricePerDay.Value, dateNow, dateNow)
                                });
                            }
                        }
                        CardDynamics = DateViews;
                    } else if (Session.carts.Count == CardDynamics.Count)
                    {
                        DateViews = CardDynamics;
                    } else
                    {
                        foreach (DateView dateView in DateViews.ToList())
                        {
                            if (!Session.carts.Any(dv => dv.RoomId == dateView.Room.RoomId))
                            {
                                DateViews.Remove(dateView);
                            }
                        }
                        CardDynamics = DateViews;
                    }
                    
                }
                else
                {
                    // Initialize DateViews from scratch if CardDynamics is not available
                    foreach (var cart in Session.carts)
                    {
                        DateViews.Add(new DateView
                        {
                            Room = cart,
                            StartDate = dateNow,
                            EndDate = dateNow,
                            ActualPrice = GetPriceFromStartDateToEndDate(cart.RoomPricePerDay.Value, dateNow, dateNow)
                        });
                    }
                    CardDynamics = DateViews;
                }

                TotalPrice = DateViews.Sum(dv => dv.ActualPrice);
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

            //if (!CheckDate(DateView.StartDate.Value, DateView.EndDate.Value))
            //{
            //    ModelState.AddModelError("", "End date must be greater than start date.");
            //    OnGet();
            //    return Page();
            //}

            if (Session.carts == null || Session.carts.Count == 0)
            {
                // Handle empty cart case
                ModelState.AddModelError(string.Empty, "Your cart is empty.");
                return Page();
            }

            BookingReservation bookingReservation = GetBookingReservation();

            List<BookingDetail> bookingDetails = new List<BookingDetail>();

            DateViews = CardDynamics;
            TotalPrice = DateViews.Sum(dv => dv.ActualPrice);
            if (DateViews.Count() > 0)
            {
                foreach (DateView dataView in DateViews)
                {
                    BookingDetail bookingDetail = GetBookingDetail(bookingReservation.BookingReservationId
                        , dataView);
                    bookingDetails.Add(bookingDetail);
                }

                bookingReservation.TotalPrice = TotalPrice;
                //Add
                await _bookingReservationService.AddBookingReservation(bookingReservation);
                await _bookingDetailService.AddRangeBookingDetail(bookingDetails);

                TempData["Message"] = "Booking successfully added!";

            }
            else
            {
                ModelState.AddModelError("", "Pls add more room.");
            }

            // Clear the cart after successful checkout
            Session.carts.Clear();
            CardDynamics.Clear();

            // Redirect to a confirmation page or home page
            return Redirect("/BookingReservations/Index");
        }

        private BookingReservation GetBookingReservation()
        {
            var email = HttpContext.Session.GetString("Username").ToLower().ToString();
            Customer customer = _customerService.GetCustomerByQueryable(m => m.EmailAddress == email).Result;
            return new BookingReservation
            {
                BookingReservationId = _bookingReservationService.GetMaxBookingIdAsync().Result + 1,
                BookingDate = DateOnly.FromDateTime(DateTime.UtcNow),
                TotalPrice = 0,
                CustomerId = customer.CustomerId,
                BookingStatus = Convert.ToByte(1),
            };
        }

        private BookingDetail GetBookingDetail(int bookingReservationId, DateView dateView)
        {
            return new BookingDetail
            {
                BookingReservationId = bookingReservationId,
                RoomId = dateView.Room.RoomId,
                StartDate = dateView.StartDate,
                EndDate = dateView.EndDate,
                ActualPrice = dateView.ActualPrice,
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

        public JsonResult OnPostUpdateActualPrice([FromBody]UpdateActualPrice updateActualPrice)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            if (DateTime.TryParseExact(updateActualPrice.startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate) &&
                DateTime.TryParseExact(updateActualPrice.endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate))
            {
                if(!CheckDate(DateOnly.FromDateTime(parsedStartDate.Date), DateOnly.FromDateTime(parsedEndDate.Date))){
                    OnGet();
                    Response.StatusCode = 400; // Bad request
                    return new JsonResult(new
                    {
                        error = "Invalid End date must be larger Start date.",
                        resetStartDate = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd"),
                        resetEndDate = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd")
                    }, options);
                }

                var room = CardDynamics[updateActualPrice.index].Room;
                decimal? roomPricePerDay = room.RoomPricePerDay;

                if (roomPricePerDay.HasValue)
                {
                    int numberOfDays = (int)(parsedEndDate.Date - parsedStartDate.Date).TotalDays + 1;
                    decimal actualPrice = roomPricePerDay.Value * numberOfDays;

                    // update actual price
                    CardDynamics[updateActualPrice.index].StartDate = DateOnly.FromDateTime(parsedStartDate.Date);
                    CardDynamics[updateActualPrice.index].EndDate = DateOnly.FromDateTime(parsedEndDate.Date);
                    CardDynamics[updateActualPrice.index].ActualPrice = actualPrice;
                    DateViews = CardDynamics;
                    TotalPrice = DateViews.Sum(dv => dv.ActualPrice);
                    return new JsonResult(new { actualPrice = actualPrice });
                }
            }

            // Return an error response if parsing dates or calculating price fails
            Response.StatusCode = 400; // Bad request
            return new JsonResult(new { error = "Invalid dates or room price." }, options);
        }
    }
}
