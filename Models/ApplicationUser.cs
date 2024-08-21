using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Visitors.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FulllName => $"{FirstName} {MiddleName} {LastName}";
    }
}
