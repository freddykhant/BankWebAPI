using BankFrontEndJS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            //Response.Cookies.Delete("SessionID");
            return PartialView("Transactions");
        }

        [HttpGet]
        [Route("/gettransactions")]
        public IActionResult GetTransactions() 
        {
            List<string> info = new List<string>();

            //List<Transaction> transactions = LoginController.getTransactions();
            RestClient restClient = new RestClient("http://localhost:5246");
            RestRequest restRequest = new RestRequest("/api/transaction/byaccnum/" + LoginController.getCurrentAccounts()[0].AccountNumber, Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(restResponse.Content);

            restRequest = new RestRequest("/api/transaction/byaccnum/" + LoginController.getCurrentAccounts()[1].AccountNumber, Method.Get);
            restResponse = restClient.Execute(restRequest);
            // transactions from the second account
            List<Transaction> transactions1 = JsonConvert.DeserializeObject<List<Transaction>>(restResponse.Content);

            foreach (Transaction singleTransaction in transactions1)
            {
                if (singleTransaction != null)
                {
                    transactions.Add(singleTransaction);
                }
            }

            foreach (Transaction transaction in transactions)
            {
                //TransactionType type = transaction.Type;
                info.Add(transaction.Type.ToString());
                info.Add(transaction.Amount.ToString());
                info.Add(transaction.Timestamp.ToString());
                info.Add(transaction.AccountNumber.ToString());
            }

            return Ok(info);
        }

        /*{
            "TransactionId": 9999,
            "AccountNumber": 1000,
            "Type": 0,
            "Amount": 300.00,
            "Timestamp": "2023-10-24T15:18:02",
            "Account": null
        }*/

        [HttpPost]
        public void CreateTransaction([FromBody] Transaction inTransaction)
        {

            Transaction transaction = new Transaction();
            transaction.TransactionId = inTransaction.TransactionId;
            transaction.AccountNumber = inTransaction.AccountNumber;
            transaction.Type = inTransaction.Type;
            transaction.Amount = inTransaction.Amount;
            transaction.Timestamp = inTransaction.Timestamp;
            transaction.Account = null;

            RestClient restClient = new RestClient("http://localhost:5246");
            RestRequest restRequest = new RestRequest("/api/transaction", Method.Post);
            restRequest.AddJsonBody(transaction);
            RestResponse restResponse = restClient.Execute(restRequest);

        }
    }
}
