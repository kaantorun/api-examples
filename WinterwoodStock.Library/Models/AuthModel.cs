using WinterwoodStock.Library.Enums;

namespace WinterwoodStock.Library.Models
{
    public class AuthModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public StatusMessage Status { get; set; }
        public string Message { get; set; }

        //default is success
        public AuthModel() { Status = StatusMessage.Success; }
    }
}
