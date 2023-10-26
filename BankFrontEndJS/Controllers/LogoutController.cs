using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            Response.Cookies.Delete("SessionID");
            return PartialView("LogoutView");
        }
    }
}

