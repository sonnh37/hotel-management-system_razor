using FHS.DataAccess.Entities;

namespace FHS.BusinessLogic.Views
{
    public class DateView
    {
        public DateOnly StartDate { get; set; } 

        public DateOnly EndDate { get; set; }

        public decimal? ActualPrice { get; set; }

        public RoomInformation Room { get; set; }
    }
}
