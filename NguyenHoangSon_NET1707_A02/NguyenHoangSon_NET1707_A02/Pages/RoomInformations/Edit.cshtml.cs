using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.RoomInformations
{
    public class EditModel : PageModel
    {
        private readonly RoomInformationService _roomInformationService;
        private readonly RoomTypeService _roomTypeService;
        private readonly IMapper _mapper;

        public EditModel(RoomInformationService roomInformationService, RoomTypeService roomTypeService, IMapper mapper)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
            _mapper = mapper;
        }

        [BindProperty]
        public RoomInformationView RoomInformation { get; set; } = default!;

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
            RoomInformation = _mapper.Map<RoomInformationView>(roominformation);
            ViewData["RoomTypeId"] = new SelectList(await _roomTypeService.GetAllRoomType(), "RoomTypeId", "RoomTypeName");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _roomInformationService.UpdateRoomInformation(_mapper.Map<RoomInformation>(RoomInformation));
            TempData["Message"] = "Update Succesfully";
            return RedirectToPage("./Index");
        }
    }
}
