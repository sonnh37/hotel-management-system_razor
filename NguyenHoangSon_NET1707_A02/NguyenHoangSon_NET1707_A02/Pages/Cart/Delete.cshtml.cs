using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.Cart
{
    public class DeleteModel : PageModel
    {
        private readonly RoomInformationService _roomInformationService;

        public DeleteModel(RoomInformationService roomInformationService)
        {
            _roomInformationService = roomInformationService;
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roominformation = await _roomInformationService.GetRoomInformationByQueryable(m => m.RoomId == id);

            if (roominformation == null)
            {
                return NotFound();
            }
            else
            {
                RoomInformation = roominformation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the room information based on the provided id
            var roominformation = await _roomInformationService.GetRoomInformationByQueryable(m => m.RoomId == id);

            if (roominformation != null && Session.carts != null)
            {
                // Find the room in the cart list using RoomId
                var itemToRemove = Session.carts.SingleOrDefault(r => r.RoomId == roominformation.RoomId);

                // If the item is found, remove it
                if (itemToRemove != null)
                {
                    Session.carts.Remove(itemToRemove);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
