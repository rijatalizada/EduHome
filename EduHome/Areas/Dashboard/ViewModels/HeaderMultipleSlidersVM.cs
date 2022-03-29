using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Dashboard.ViewModels;

public class HeaderMultipleSlidersVM
{
    [Required(ErrorMessage = "Please Enter Title")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Please Enter SubTitle")]
    public string TitleH2 { get; set; }
    [Required]
    public string Description { get; set; }
    
    public IFormFile[] Images { get; set; }
}