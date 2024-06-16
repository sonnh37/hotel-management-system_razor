namespace FHS.BusinessLogic.Views
{
    public class BookingReservationView
    {
        public int BookingReservationId { get; set; }

        public DateOnly? BookingDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public int CustomerId { get; set; }

        public byte? BookingStatus { get; set; }

        public IList<BookingDetailView> BookingDetails { get; set; } = new List<BookingDetailView>();

        public CustomerView? Customer { get; set; }
    }
}
