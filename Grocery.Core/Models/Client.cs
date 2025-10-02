
namespace Grocery.Core.Models
{
    public partial class Client : Model
        
    {

        public enum Role
        {
            None,
            Admin
        }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role userRole { get; set; } = Role.None;

        public Client(int id, string name, string emailAddress, string password, Role userRole) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            this.userRole = userRole;
        }
    }
}
