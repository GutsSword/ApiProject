using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
