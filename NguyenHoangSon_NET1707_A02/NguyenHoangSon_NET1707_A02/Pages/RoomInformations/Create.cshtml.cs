using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.RoomInformations
{
    public class CreateModel : PageModel
    {
        private readonly RoomInformationService _roomInformationService;
        private readonly RoomTypeService _roomTypeService;
        private readonly IMapper _mapper;

        public CreateModel(RoomInformationService roomInformationService, RoomTypeService roomTypeService, IMapper mapper)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["RoomTypeId"] = new SelectList(await _roomTypeService.GetAllRoomType(), "RoomTypeId", "RoomTypeName");
            return Page();
        }

        [BindProperty]
        public RoomInformationView RoomInformation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var room = await _roomInformationService.AddRoomInformation(_mapper.Map<RoomInformation>(RoomInformation));
            if (room == null)
            {
                ModelState.AddModelError("", "Error while adding room");
                return Page();
            }
            TempData["Message"] = "Add Succesfully";
            return RedirectToPage("./Index");
        }
    }
}
