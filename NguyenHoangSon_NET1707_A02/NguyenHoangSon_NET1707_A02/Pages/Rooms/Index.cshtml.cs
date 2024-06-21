using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using AutoMapper;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly RoomInformationService _roomInformationService;
        private readonly RoomTypeService _roomTypeService;
        private readonly IMapper _mapper;

        public IndexModel(RoomInformationService roomInformationService, RoomTypeService roomTypeService, IMapper mapper)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
            _mapper = mapper;
        }

        public IList<RoomInformation> RoomInformation { get;set; } = default!;

        public string txtSearch { get;set; }

        public async Task OnGetAsync()
        {
            RoomInformation = await _roomInformationService.GetAllRoomInformation();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Fetch the room information based on the provided id
            var roomInformation = await _roomInformationService.GetRoomInformationByQueryable(m => m.RoomId == id);

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
                    ModelState.AddModelError("", $"Room {roomInformation.RoomNumber} was added. Pls choose another!");
                    await OnGetAsync();
                    return Page();
                }
            }

            Session.carts.Add(roomInformation);
            TempData["Message"] = ($"Room {roomInformation.RoomNumber} added to cart successfully!");
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync(string txtSearch)
        {
            if (string.IsNullOrEmpty(txtSearch))
            {
                await OnGetAsync();
                return Page();
            }

            this.txtSearch = txtSearch;
            RoomInformation = await _roomInformationService.GetRoomInformationListByQueryable(m => m.RoomDetailDescription.ToLower().Trim().Contains(txtSearch.ToLower().Trim()));

            return Page();
        }
    }
}
