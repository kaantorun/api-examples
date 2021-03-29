using System;

namespace WinterwoodStock.Library.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
