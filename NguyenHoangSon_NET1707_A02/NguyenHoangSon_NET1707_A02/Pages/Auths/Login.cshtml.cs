using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenHoangSon_NET1707_A02.Data;
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
            //if (HttpContext.Session.GetString("Username") == null)
            //{
            //    return Page();
            //}

            //return Redirect("/");
        }

        public IActionResult OnPost()
        {
            var user = _context.Customers.Where(m => m.EmailAddress == LoginView.Email && m.Password == LoginView.Password).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");

                return Page();
                
            }
            HttpContext.Session.SetString("Username", user.EmailAddress);

            return Redirect("/");
        }
    }
}
