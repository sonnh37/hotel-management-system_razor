using AutoMapper;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Tools
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
