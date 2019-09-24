using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portal.Domain.Identity;

namespace Portal.Web.Areas.User.Pages.Profile
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            //FirstName = user.FirstName;
            //LastName = user.LastName;
            Email = user.Email;

            return Page();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            var firstNameClaim = new Claim(nameof(FirstName), FirstName);
            var lastNameClaim = new Claim(nameof(LastName), LastName);
            var claimsList = new List<Claim>
            {
                firstNameClaim,
                lastNameClaim
            };

            var claims = (await _userManager.GetClaimsAsync(user))
               .Where(c => c.Type == nameof(FirstName) || c.Type == nameof(LastName));

            await _userManager.RemoveClaimsAsync(user, claims);
            await _userManager.UpdateAsync(user);

            await _userManager.AddClaimsAsync(user, claimsList); 
            await _userManager.UpdateAsync(user);

        

            return RedirectToPage("./index");
        }
    }
}