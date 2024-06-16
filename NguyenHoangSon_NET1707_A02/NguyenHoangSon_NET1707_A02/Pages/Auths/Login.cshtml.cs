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

        public LoginModel(CustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            Customer user = null;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var account = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("account");
            if (LoginView.Email.ToLower().Equals(account["email"].ToLower()) && LoginView.Password.Equals(account["password"]))
            {
                HttpContext.Session.SetString("Role", "Admin");
                user = new Customer();
                user.EmailAddress = account["email"].ToLower();
            }
            else
            {
                user = await _customerService.GetCustomerByQueryable(m => m.EmailAddress == LoginView.Email && m.Password == LoginView.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");

                    return Page();
                }
            }

            HttpContext.Session.SetString("Username", user.EmailAddress);

            return Redirect("/");
        }
    }
}
