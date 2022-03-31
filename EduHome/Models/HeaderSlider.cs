using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.Models;

public class HeaderSlider
{
    public int Id { get; set; }
    public string Image { get; set; }
    [Required(ErrorMessage = "Please Enter Title")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Please Enter SubTitle")]
    public string TitleH2 { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public byte? Order { get; set; }  
    
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    
}