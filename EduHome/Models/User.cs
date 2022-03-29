using Microsoft.AspNetCore.Identity;

namespace EduHome.Models;

public class User : IdentityUser
{
    public string Fullname { get; set; }
    public byte Age { get; set; }
    public string Position { get; set; }
}