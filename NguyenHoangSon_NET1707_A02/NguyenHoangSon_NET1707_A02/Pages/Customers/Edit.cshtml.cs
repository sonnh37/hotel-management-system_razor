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

namespace NguyenHoangSon_NET1707_A02.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;

        public EditModel(CustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [BindProperty]
        public CustomerView Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer =  await _customerService.GetCustomerByQueryable(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            Customer = _mapper.Map<CustomerView>(customer);
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

            await _customerService.UpdateCustomer(_mapper.Map<Customer>(Customer));
            TempData["Message"] = "Update Succesfully";
            return RedirectToPage("./Index");
        }
    }
}
