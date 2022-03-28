using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class HomeNoticePage
{
    public int Id { get; set; }
    public DateTime PostDate { get; set; }
    [Required(ErrorMessage = "Please Enter Notice Title")]
    public string Description { get; set; }
    
}