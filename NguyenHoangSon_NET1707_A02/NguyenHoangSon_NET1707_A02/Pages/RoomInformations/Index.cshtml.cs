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
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NguyenHoangSon_NET1707_A02.Pages.RoomInformations
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

        [BindProperty]
        public int totalPages { get; set; } = 1;

        [BindProperty]
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 5;

        public JsonResult OnGetSignalR(int pageNumber = 1)
        {
            GetAll(pageNumber).Wait();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            return new JsonResult(RoomInformation, options);
        }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            this.pageNumber = pageNumber;
            await GetAll(pageNumber);
        }

        public async Task GetAll(int pageNumber)
        {
            var item = await _roomInformationService.GetAllRoomInformation(pageNumber, this.pageSize);
            RoomInformation = item.Item1;
            totalPages = item.Item2;
        }
    }
}
