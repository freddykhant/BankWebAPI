namespace BankWebAPI.Models
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte[] Picture { get; set; } 
        public string Password { get; set; } 

        public string UserNumber { get; set; }
    }

}
