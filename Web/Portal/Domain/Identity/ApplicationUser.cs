using Microsoft.AspNetCore.Identity;
using Portal.Domain.Enums;

namespace Portal.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public GenderType Gender { get; set; }
    }
}
