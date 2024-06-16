namespace FHS.BusinessLogic.Views
{
    public class RoomTypeView
    {
        public int RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = null!;

        public string? TypeDescription { get; set; }

        public string? TypeNote { get; set; }

        public IList<RoomInformationView> RoomInformations { get; set; } = new List<RoomInformationView>();
    }
}
