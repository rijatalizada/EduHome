using Microsoft.AspNetCore.Identity;

namespace EduHome.Models;

public class User : IdentityUser
{
    public string FullName { get; set; }
    public byte Age { get; set; }
    public string Position { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Comment> Comments { get; set; }
}