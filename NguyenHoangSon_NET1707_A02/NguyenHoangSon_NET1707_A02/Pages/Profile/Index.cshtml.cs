using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using FHS.DataAccess.Entities;
using FHS.BusinessLogic.Views;
using FHS.BusinessLogic.Services;

namespace NguyenHoangSon_NET1707_A02.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;

        public IndexModel(CustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [BindProperty]
        public CustomerView CustomerView { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var email = HttpContext.Session.GetString("Username");
            if (email != null)
            {
                var customer = await _customerService.GetCustomerByQueryable(m => m.EmailAddress == email);

                if (customer != null)
                {
                    CustomerView = _mapper.Map<CustomerView>(customer);
                    return;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _customerService.UpdateCustomer(_mapper.Map<Customer>(CustomerView));
            TempData["Message"] = ($"Update Succesfully.");
            return Page();
        }

    }
}
