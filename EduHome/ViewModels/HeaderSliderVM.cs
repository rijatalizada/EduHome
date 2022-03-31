using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewModels;

public class HeaderSliderVM
{

    [Required(ErrorMessage = "Please Enter Title")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Please Enter SubTitle")]
    public string TitleH2 { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public byte? Order { get; set; }  
    public IFormFile ImageFile { get; set; }
}