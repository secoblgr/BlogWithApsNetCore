using Microsoft.AspNetCore.Identity;

namespace BlogV1.Identity
{
    public class BlogIdentityUser : IdentityUser    // kütüphane bagladık.
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
