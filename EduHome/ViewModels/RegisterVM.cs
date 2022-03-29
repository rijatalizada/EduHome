using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class RegisterVM
{
    [Required]
    public string Fullname { get; set; }
    [Required, MaxLength(30)]    
    public string Username { get; set; }
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, Range(10, 120)]
    public byte Age { get; set; }
    [Required]
    public string Position { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}