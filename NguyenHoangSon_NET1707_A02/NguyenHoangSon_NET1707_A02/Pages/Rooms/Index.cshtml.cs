using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;

namespace NguyenHoangSon_NET1707_A02.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly NguyenHoangSon_NET1707_A02.Data.FuminiHotelManagementContext _context;

        public IndexModel(NguyenHoangSon_NET1707_A02.Data.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IList<RoomInformation> RoomInformation { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RoomInformation = await _context.RoomInformations
                .Include(r => r.RoomType).Where(m => m.RoomStatus == Convert.ToByte(1) ).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Fetch the room information based on the provided id
            var roomInformation = await _context.RoomInformations.FindAsync(id);

            if (roomInformation == null)
            {
                return NotFound();
            }
            
            if (Session.carts == null)
            {
                Session.carts = new List<RoomInformation>();
            }

            foreach (RoomInformation x in Session.carts)
            {
                if (x.RoomId == roomInformation.RoomId)
                {
                    TempData["Message"] = ($"Room {roomInformation.RoomNumber} was added. Pls choose another!");
                    await OnGetAsync();
                    return Page();
                }
            }

            Session.carts.Add(roomInformation);
            TempData["Message"] = ($"Room {roomInformation.RoomNumber} added to cart successfully!");
            await OnGetAsync();
            return Page();
        }
    }
}
