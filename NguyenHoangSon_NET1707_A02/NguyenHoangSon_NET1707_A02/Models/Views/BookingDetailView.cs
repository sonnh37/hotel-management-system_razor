namespace NguyenHoangSon_NET1707_A02.Models.Views
{
    public class BookingDetailView
    {
        public int BookingReservationId { get; set; }

        public int RoomId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public decimal? ActualPrice { get; set; }

        public BookingReservationView? BookingReservation { get; set; }

        public RoomInformation? Room { get; set; }
    }
}
