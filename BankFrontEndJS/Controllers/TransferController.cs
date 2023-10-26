using BankFrontEndJS.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class TransferController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            //Response.Cookies.Delete("SessionID");

            // what I need
            // the user's 2 accounts

            List<Account> accounts = LoginController.getCurrentAccounts();

            List<string> info = new List<string>();

            info.Add(accounts[0].AccountNumber.ToString());
            info.Add(accounts[1].AccountNumber.ToString());

            return PartialView("TransferMoney", info);
        }
    }
}
