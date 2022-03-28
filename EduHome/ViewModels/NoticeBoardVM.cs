using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class NoticeBoardVM
{
    [Required(ErrorMessage = "Please Enter Notice Title")]
    public string Description { get; set; }
    
    public DateTime PostDate { get; set; }
    
}