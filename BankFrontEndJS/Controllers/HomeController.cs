using BankFrontEndJS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankFrontEndJS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}