using BankFrontEndJS.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            //Response.Cookies.Delete("SessionID");
            //List<String> lol = new List<String>();

            // name (and profile picture)
            // contact info: email, phone number
            // need options to change stuff

            // thinking for now I'll just make a request to get stuff each time but
            // could also request everything upon login and store it so doesn't have to request
            // every time a button is pressed

            UserProfile user = LoginController.getCurrentUser();

            List<string> info = new List<string>();
            info.Add(user.Name);
            info.Add(user.Email);
            info.Add(user.Phone);


            return PartialView("Profile", info);
        }

        [HttpPost]
        public void UpdateProfile([FromBody] User user)
        {
            // db update first

            UserProfile userPofile = LoginController.getCurrentUser();

            //int isName = 0;

            string[] parts = user.UserName.Split('+');
            if (parts[1].Equals("name"))
            {
                userPofile.Name = parts[0];
            } else if (parts[1].Equals("email"))
            {
                userPofile.Email = parts[0];
            } else if (parts[1].Equals("number"))
            {
                userPofile.Phone = parts[0];
            }

            // probably gonna get a fat error if we try to change the name
            // because the db updates by the name lol 

            RestClient restClient = new RestClient("http://localhost:5246");
            RestRequest restRequest = new RestRequest("/api/userprofile", Method.Put);
            restRequest.AddJsonBody(userPofile);
            RestResponse restResponse = restClient.Execute(restRequest);

            //UserProfile userProfile;
            // then update for website
            LoginController.setCurrentUser(userPofile);

            //GetView();
        }
    }
}
