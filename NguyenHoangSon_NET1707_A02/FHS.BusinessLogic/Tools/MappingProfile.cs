
using AutoMapper;
using FHS.BusinessLogic.Views;
using FHS.DataAccess.Entities;

namespace FHS.BusinessLogic.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomInformation, RoomInformationView>().ReverseMap();
            CreateMap<RoomType, RoomTypeView>().ReverseMap();
            CreateMap<Customer, CustomerView>().ReverseMap();
            CreateMap<BookingDetail, BookingDetailView>().ReverseMap();
            CreateMap<BookingReservation, BookingReservationView>().ReverseMap();
        }
    }
}
