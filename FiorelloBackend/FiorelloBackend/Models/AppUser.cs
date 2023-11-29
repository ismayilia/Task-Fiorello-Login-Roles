using Microsoft.AspNetCore.Identity;

namespace FiorelloBackend.Models
{
    public class AppUser : IdentityUser 
    {
        public string FullName { get; set; }
    }
}
