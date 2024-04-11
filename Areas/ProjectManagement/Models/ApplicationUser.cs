

using Microsoft.AspNetCore.Identity;
namespace WebApplication2.Areas.ProjectManagement.Models
{
    public class ApplicationUser: IdentityUser
    {

        public String FirstName { get; set; }
        public string LastName { get; set; }

        public int UserNameChangeLimit { get; set; } = 10;

        public byte[]? ProfilePicture { get; set; }
    }
}
