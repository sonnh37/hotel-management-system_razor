namespace FHS.BusinessLogic.Views
{
    public class RoomInformationView
    {
        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = null!;

        public string? RoomDetailDescription { get; set; }

        public int? RoomMaxCapacity { get; set; }

        public int RoomTypeId { get; set; }

        public byte? RoomStatus { get; set; }

        public decimal? RoomPricePerDay { get; set; }

        public IList<BookingDetailView> BookingDetails { get; set; } = new List<BookingDetailView>();

        public RoomTypeView? RoomType { get; set; }
    }
}
