using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;

namespace NguyenHoangSon_NET1707_A02.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;

        public IndexModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public IList<RoomInformation> RoomInformation { get; set; } = new List<RoomInformation>();

        public void OnGet()
        {
            if (Session.carts.Count > 0)
            {
                RoomInformation = Session.carts;
            }
        }
    }
}
