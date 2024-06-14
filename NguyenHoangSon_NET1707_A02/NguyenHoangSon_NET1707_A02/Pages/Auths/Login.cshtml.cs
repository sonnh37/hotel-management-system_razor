using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.Auths
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginView LoginView { get; set; } = default!;

        private readonly FuminiHotelManagementContext _context;

        public LoginModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
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
                Console.Write(HttpContext.Session.GetString("Role"));
            }
            else
            {
                user = _context.Customers.Where(m => m.EmailAddress == LoginView.Email && m.Password == LoginView.Password).FirstOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");

                    return Page();

                }
            }

            HttpContext.Session.SetString("Username", user.EmailAddress);

            return Redirect("/");
        }
    }
}
