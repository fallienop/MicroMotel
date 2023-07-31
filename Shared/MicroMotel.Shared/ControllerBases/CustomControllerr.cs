using MicroMotel.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MicroMotel.Shared.ControllerBases
{
    public class CustomControllerr:ControllerBase
    {
        public IActionResult CustomActionResult<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.Status
            };
        }
    }
}
