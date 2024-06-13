using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenHoangSon_NET1707_A02.Pages.Auths
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Clear the session
            HttpContext.Session.Clear();

            return Redirect("/Auths/Login");
        }
    }
}
