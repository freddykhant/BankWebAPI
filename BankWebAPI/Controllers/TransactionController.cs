using BankWebAPI.Data;
using BankWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static BankWebAPI.Models.Transaction;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        [HttpGet]
        public IEnumerable<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = DBManager.GetAllTransactions();
            return transactions;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTransactionById(int id)
        {
            var transaction = DBManager.GetTransactionById(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        [HttpGet]
        [Route("byaccnum/{accountNumber}")]
        public IActionResult GetTransactionByAccountNumber(int accountNumber)
        {
            var transactions = DBManager.GetTransactionByAccountNumber(accountNumber);
            if (transactions == null)
                return NotFound();

            return Ok(transactions);
        }

        /*[HttpGet]
        [Route("byusernum/{accountNumber}")]
        public IActionResult GetTransactionByUserNumber(int userNumber)
        {
            var transactions = DBManager.GetTransactionsByUserNumber(userNumber);
            if (transactions == null)
                return NotFound();

            return Ok(transactions);
        }*/


        // call like this (type in int)
        /*{
            "TransactionId": 9999,
            "AccountNumber": 1000,
            "Type": 0,
            "Amount": 300.00,
            "Timestamp": "2023-10-24T15:18:02",
            "Account": null
        }*/


        [HttpPost]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            Account account = DBManager.GetAccountByNumber(transaction.AccountNumber);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            if (transaction.Type == TransactionType.Withdrawal && account.Balance < transaction.Amount)
            {
                return BadRequest("Insufficient funds.");
            }

            if (transaction.Type == TransactionType.Deposit)
            {
                account.Balance += transaction.Amount;
            }
            else
            {
                account.Balance -= transaction.Amount;
            }

            DBManager.UpdateAccount(account);

            if (DBManager.InsertTransaction(transaction))
                return Ok("Successfully inserted");
            return BadRequest("Error in data insertion");
        }


        [HttpPut]
        public IActionResult UpdateTransaction([FromBody] Transaction transaction)
        {
            if (DBManager.UpdateTransaction(transaction))
                return Ok("Successfully updated");
            return BadRequest("Could not update");
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            if (DBManager.DeleteTransaction(id))
                return Ok("Successfully Deleted");
            return BadRequest("Could not delete");
        }
    }
}
