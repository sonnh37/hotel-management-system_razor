using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.RoomInformations
{
    public class CreateModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly IMapper _mapper;

        public CreateModel(FuminiHotelManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName");
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

            _context.RoomInformations.Add(_mapper.Map<RoomInformation>(RoomInformation));
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
