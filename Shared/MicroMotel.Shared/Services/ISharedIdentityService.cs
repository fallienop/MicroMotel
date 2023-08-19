using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroMotel.Shared.Services
{
    public interface ISharedIdentityService
    {
        public string getUserId { get; }
        //public string getUserName { get; }
    }
}
