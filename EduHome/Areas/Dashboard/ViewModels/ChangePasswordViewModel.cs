using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Dashboard.ViewModels;

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), MaxLength(15)]
    public string OldPassword { get; set; }

    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; }
    
}