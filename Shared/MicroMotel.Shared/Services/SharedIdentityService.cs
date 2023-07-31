using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMotel.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpcontextaccessor;

        public SharedIdentityService(IHttpContextAccessor httpcontextaccessor)
        {
            _httpcontextaccessor = httpcontextaccessor;
        }

        public string getUserId
        {
            get
            {
                var user=_httpcontextaccessor.HttpContext?.User;
                if (user != null)
                {
                    var userIdClaim = user.FindFirst("sub");
                    if (userIdClaim != null)
                    {
                        return userIdClaim.Value;
                    }
                }

                return null;
            }

        }
    }
}
