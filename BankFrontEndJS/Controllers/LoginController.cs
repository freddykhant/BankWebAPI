
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using BankFrontEndJS.Models;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private static UserProfile currentUser;
        private static List<Account> currentAccounts;
        private static List<Transaction> transactions;
        public static UserProfile getCurrentUser() { return currentUser; }

        public static List<Account> getCurrentAccounts() { return currentAccounts; }

        public static void setCurrentUser(UserProfile user) { currentUser = user; }

        public static List<Transaction> getTransactions() { return transactions; }

        public static void addTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
        }


        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("LoginViewAuthenticated");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginDefaultView");
        }

        [HttpGet("authview")]
        public IActionResult GetLoginAuthenticatedView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("LoginViewAuthenticated");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginErrorView");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("LoginErrorView");
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var response = new { login = false };

            RestClient restClient = new RestClient("http://localhost:5246");
            RestRequest restRequest = new RestRequest("/api/userprofile/email/" + user.UserName, Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            UserProfile userProfile;

            userProfile = JsonConvert.DeserializeObject<UserProfile>(restResponse.Content);

            if (userProfile != null)
            {
                if (user != null && user.PassWord.Equals(userProfile.Password))
                {
                    currentUser = userProfile;

                    // get account
                    restRequest = new RestRequest("/api/account/byusernum/" + currentUser.UserNumber, Method.Get);
                    restResponse = restClient.Execute(restRequest);
                    currentAccounts = JsonConvert.DeserializeObject<List<Account>>(restResponse.Content);

                    Response.Cookies.Append("SessionID", "1234567");
                    response = new { login = true };
                }
            }
            return Json(response);

        }
    }
}
