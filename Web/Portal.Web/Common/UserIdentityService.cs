using Microsoft.AspNetCore.Http;
using Portal.Persistance.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Portal.Web.Common
{
    public class UserIdentityService : IUserIdentityService
    {

        private readonly IHttpContextAccessor _accessor;
        public UserIdentityService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetUserId()
        {
            return _accessor.HttpContext?
               .User
               .Claims
                .SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;
        }
    }
}
