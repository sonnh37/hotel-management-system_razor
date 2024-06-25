using AutoMapper;
using FHS.BusinessLogic.Services;
using FHS.BusinessLogic.Views;
using FHS.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace NguyenHoangSon_NET1707_A02.Pages.Auths
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginView LoginView { get; set; } = default!;

        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoginModel(CustomerService customerService, IMapper mapper, IConfiguration configuration)
        {
            _customerService = customerService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var adminEmail = _configuration["Account:Email"];
            var adminPassword = _configuration["Account:Password"];
            Customer user = null;

            if (LoginView.Email.Equals(adminEmail, System.StringComparison.OrdinalIgnoreCase) &&
                LoginView.Password.Equals(adminPassword))
            {
                HttpContext.Session.SetString("Role", "Admin");
                user = new Customer { EmailAddress = adminEmail.ToLower().Trim() };
            }
            else
            {
                user = await _customerService.GetCustomerByQueryable(m =>
                    m.EmailAddress == LoginView.Email && m.Password == LoginView.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return Page();
                }
            }

            HttpContext.Session.SetString("Username", user.EmailAddress);
            return RedirectToPage("/Index");
        }
    }
}
