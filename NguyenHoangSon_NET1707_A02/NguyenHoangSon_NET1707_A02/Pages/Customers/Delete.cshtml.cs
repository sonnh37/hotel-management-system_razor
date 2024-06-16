using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Services;
using AutoMapper;

namespace NguyenHoangSon_NET1707_A02.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;

        public DeleteModel(CustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerByQueryable(m => m.CustomerId == id);
            Customer = customer;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _customerService.DeleteCustomer(id.Value);
            TempData["Message"] = "Delete Succesfully";
            return RedirectToPage("./Index");
        }
    }
}
