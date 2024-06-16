using FHS.DataAccess.Entities;
using System;
using System.Linq;

namespace FHS.DataAccess.Entities.InitData
{
    public class DbInitializer
    {
        public static void Initialize(FuminiHotelManagementContext context)
        {
            context.Database.EnsureCreated();

            // Check if the database has been seeded
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            var customers = new Customer[]
            {
                new Customer{CustomerFullName="William Shakespeare", Telephone="0903939393", EmailAddress="WilliamShakespeare@FUMiniHotel.org", CustomerBirthday=DateOnly.FromDateTime(DateTime.Parse("1990-02-02")), CustomerStatus=1, Password="123@"},
                new Customer{CustomerFullName="Elizabeth Taylor", Telephone="0903939377", EmailAddress="ElizabethTaylor@FUMiniHotel.org", CustomerBirthday=DateOnly.FromDateTime(DateTime.Parse("1991-03-03")), CustomerStatus=1, Password="144@"},
                new Customer{CustomerFullName="James Cameron", Telephone="0903946582", EmailAddress="JamesCameron@FUMiniHotel.org", CustomerBirthday=DateOnly.FromDateTime(DateTime.Parse("1992-11-10")), CustomerStatus=1, Password="443@"},
                new Customer{CustomerFullName="Charles Dickens", Telephone="0903955633", EmailAddress="CharlesDickens@FUMiniHotel.org", CustomerBirthday=DateOnly.FromDateTime(DateTime.Parse("1991-12-05")), CustomerStatus=1, Password="563@"},
                new Customer{CustomerFullName="George Orwell", Telephone="0913933493", EmailAddress="GeorgeOrwell@FUMiniHotel.org", CustomerBirthday=DateOnly.FromDateTime(DateTime.Parse("1993-12-24")), CustomerStatus=1, Password="177@"},
                new Customer{CustomerFullName="Victoria Beckham", Telephone="0983246773", EmailAddress="VictoriaBeckham@FUMiniHotel.org", CustomerBirthday=DateOnly.FromDateTime(DateTime.Parse("1990-09-09")), CustomerStatus=1, Password="654@"}
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var roomTypes = new RoomType[]
            {
                new RoomType{RoomTypeName="Standard room", TypeDescription="This is typically the most affordable option and provides basic amenities such as a bed, dresser, and TV.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Suite", TypeDescription="Suites usually offer more space and amenities than standard rooms, such as a separate living area, kitchenette, and multiple bathrooms.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Deluxe room", TypeDescription="Deluxe rooms offer additional features such as a balcony or sea view, upgraded bedding, and improved décor.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Executive room", TypeDescription="Executive rooms are designed for business travelers and offer perks such as free breakfast, evening drink, and high-speed internet.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Family Room", TypeDescription="A room specifically designed to accommodate families, often with multiple beds and additional space for children.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Connecting Room", TypeDescription="Two or more rooms with a connecting door, providing flexibility for larger groups or families traveling together.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Penthouse Suite", TypeDescription="An extravagant, top-floor suite with exceptional views and exclusive amenities, typically chosen for special occasions or VIP guests.", TypeNote="N/A"},
                new RoomType{RoomTypeName="Bungalow", TypeDescription="A standalone cottage-style accommodation, providing privacy and a sense of seclusion often within a resort setting", TypeNote="N/A"}
            };
            context.RoomTypes.AddRange(roomTypes);
            context.SaveChanges();

            var roomInformations = new RoomInformation[]
            {
                new RoomInformation{RoomNumber="2364", RoomDetailDescription="A basic room with essential amenities, suitable for individual travelers or couples.", RoomMaxCapacity=3, RoomTypeId=1, RoomStatus=1, RoomPricePerDay=149.0000m},
                new RoomInformation{RoomNumber="3345", RoomDetailDescription="Deluxe rooms offer additional features such as a balcony or sea view, upgraded bedding, and improved décor.", RoomMaxCapacity=5, RoomTypeId=3, RoomStatus=1, RoomPricePerDay=299.0000m},
                new RoomInformation{RoomNumber="4432", RoomDetailDescription="A luxurious and spacious room with separate living and sleeping areas, ideal for guests seeking extra comfort and space.", RoomMaxCapacity=4, RoomTypeId=2, RoomStatus=1, RoomPricePerDay=199.0000m},
                new RoomInformation{RoomNumber="3342", RoomDetailDescription="Floor 3, Window in the North West", RoomMaxCapacity=5, RoomTypeId=5, RoomStatus=1, RoomPricePerDay=219.0000m},
                new RoomInformation{RoomNumber="4434", RoomDetailDescription="Floor 4, Full glass windows, and balcony", RoomMaxCapacity=3, RoomTypeId=1, RoomStatus=1, RoomPricePerDay=159.0000m},
                new RoomInformation{RoomNumber="4531", RoomDetailDescription="Floor 4, Sun side, small balcony", RoomMaxCapacity=4, RoomTypeId=4, RoomStatus=1, RoomPricePerDay=199.0000m}
            };
            context.RoomInformations.AddRange(roomInformations);
            context.SaveChanges();
        }
    }
}
