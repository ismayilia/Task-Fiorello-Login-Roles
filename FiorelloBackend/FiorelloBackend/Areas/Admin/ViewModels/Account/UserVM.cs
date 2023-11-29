using FiorelloBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace FiorelloBackend.Areas.Admin.ViewModels.Account
{
    public class UserVM
    {

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public IList<string> RoleName { get; set; }
    }
}
