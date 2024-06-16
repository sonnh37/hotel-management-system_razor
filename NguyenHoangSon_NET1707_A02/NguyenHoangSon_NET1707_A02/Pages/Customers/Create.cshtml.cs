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

namespace NguyenHoangSon_NET1707_A02.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateModel(CustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CustomerView Customer { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var customer = await _customerService.AddCustomer(_mapper.Map<Customer>(Customer));
            
            if (customer == null)
            {
                ModelState.AddModelError("", "Error while adding customer");
                return Page();
            }
            TempData["Message"] = "Add Succesfully.";
            return RedirectToPage("./Index");
        }
    }
}
