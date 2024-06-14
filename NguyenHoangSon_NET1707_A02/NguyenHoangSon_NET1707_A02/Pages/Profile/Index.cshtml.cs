using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NguyenHoangSon_NET1707_A02.Data;
using NguyenHoangSon_NET1707_A02.Models;
using NguyenHoangSon_NET1707_A02.Models.Views;

namespace NguyenHoangSon_NET1707_A02.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly IMapper _mapper;

        public IndexModel(FuminiHotelManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
        public CustomerView CustomerView { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var email = HttpContext.Session.GetString("Username");
            if (email != null)
            {
                var customer = await _context.Customers.Where(m => m.EmailAddress == email).SingleOrDefaultAsync();

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
                TempData["Message"] = "There was an error with the submitted data.";
                return Page();
            }

            if (CustomerView != null)
            {
                _context.Attach(_mapper.Map<Customer>(CustomerView)).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
                await OnGetAsync();
                TempData["Message"] = "Account updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(CustomerView.CustomerId))
                {
                    TempData["Message"] = "Account not found.";
                    return NotFound();
                }
                else
                {
                    TempData["Message"] = "An error occurred while updating the account.";
                    throw;
                }
            }

            return Page();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
