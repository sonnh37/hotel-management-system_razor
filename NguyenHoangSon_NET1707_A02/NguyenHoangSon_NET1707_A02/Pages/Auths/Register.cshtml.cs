using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.Auths
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public CustomerView CustomerView { get; set; } = default!;

        private readonly FuminiHotelManagementContext _context;
        private readonly IMapper _mapper;

        public RegisterModel(FuminiHotelManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            var user = _context.Customers.Where(m => m.EmailAddress == CustomerView.EmailAddress).FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "Email has exist.");

                return Page();
            }

            _context.Customers.Add(_mapper.Map<Customer>(CustomerView));
            _context.SaveChanges();

            return Redirect("/Auths/Login");
        }
    }
}
