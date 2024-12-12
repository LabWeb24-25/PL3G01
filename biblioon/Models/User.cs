using Microsoft.AspNetCore.Identity;

namespace biblioon.Models
{
    public class User : IdentityUser
    {
        public required string Nome { get; set; }
    }
}
