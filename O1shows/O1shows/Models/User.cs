using Microsoft.AspNetCore.Identity;

namespace O1shows.Models
{
    public class User : IdentityUser
    {
        public UserProfile UserProfile { get; set; }
    }
}