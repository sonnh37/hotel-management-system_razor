using AutoMapper;
using FHS.BusinessLogic.Services;
using FHS.BusinessLogic.Views;
using FHS.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenHoangSon_NET1707_A02.Pages.Auths
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public CustomerView CustomerView { get; set; } = default!;

        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;

        public RegisterModel(CustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more files has wrong.");
                return Page();
            }

            var customer = await _customerService.AddCustomer(_mapper.Map<Customer>(CustomerView));
            if (customer == null)
            {
                ModelState.AddModelError("", "Error while adding customer");
                return Page();
            }
            TempData["Message"] = "Register Succesfully.";
            return Redirect("/Auths/Login");
        }
    }
}
