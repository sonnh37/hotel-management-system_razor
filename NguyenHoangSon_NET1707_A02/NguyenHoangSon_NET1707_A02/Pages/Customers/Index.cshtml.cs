using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.Customers
{
    
    public class IndexModel : PageModel
    {
        private readonly CustomerService _customerService;

        public IndexModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public IList<Customer> Customer { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Customer = await _customerService.GetAllCustomer();
        }
    }
}
