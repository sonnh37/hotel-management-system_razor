namespace NguyenHoangSon_NET1707_A02.Models.Views
{
    public class CustomerView
    {
        public int CustomerId { get; set; }

        public string? CustomerFullName { get; set; }

        public string? Telephone { get; set; }

        public string EmailAddress { get; set; } = null!;

        public DateOnly? CustomerBirthday { get; set; }

        public byte? CustomerStatus { get; set; }

        public string? Password { get; set; }

        public IList<BookingReservationView> BookingReservations { get; set; } = new List<BookingReservationView>();
    }
}
