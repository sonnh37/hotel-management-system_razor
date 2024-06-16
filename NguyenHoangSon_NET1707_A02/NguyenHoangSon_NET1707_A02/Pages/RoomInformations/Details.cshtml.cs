using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using AutoMapper;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.RoomInformations
{
    public class DetailsModel : PageModel
    {
        private readonly RoomInformationService _roomInformationService;
        private readonly RoomTypeService _roomTypeService;
        private readonly IMapper _mapper;

        public DetailsModel(RoomInformationService roomInformationService, RoomTypeService roomTypeService, IMapper mapper)
        {
            _roomInformationService = roomInformationService;
            _roomTypeService = roomTypeService;
            _mapper = mapper;
        }

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
    }
}
