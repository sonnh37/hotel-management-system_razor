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

        [BindProperty]
        public int totalPages { get; set; } = 1;

        [BindProperty]
        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 5;

        public async Task OnGetAsync(int pageNumber = 1)
        {
            this.pageNumber = pageNumber;
            var item = await _customerService.GetAllCustomer(pageNumber, pageSize);
            Customer = item.Item1;
            totalPages = item.Item2;
        }
    }
}
