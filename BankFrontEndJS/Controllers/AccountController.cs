using Microsoft.AspNetCore.Mvc;

using BankFrontEndJS.Models;
using Newtonsoft.Json;
using RestSharp;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {

        //private List<Account> accounts;

        [HttpGet]
        public IActionResult GetView()
        {
            //Response.Cookies.Delete("SessionID");

            RestClient restClient = new RestClient("http://localhost:5246");
            RestRequest restRequest = new RestRequest("/api/account/byusernum/" + LoginController.getCurrentUser().UserNumber, Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(restResponse.Content);

            List<string> info = new List<string>();
            info.Add(accounts[0].AccountNumber.ToString());
            info.Add(accounts[0].Balance.ToString());
            info.Add(accounts[1].AccountNumber.ToString());
            info.Add(accounts[1].Balance.ToString());

            return PartialView("AccountSummary", info);
        }

    }
}
