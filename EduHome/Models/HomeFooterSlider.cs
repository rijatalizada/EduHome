using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class HomeFooterSlider
{
    public int Id { get; set; }
    
    public string Image { get; set; }
    [Required]
    [StringLength(maximumLength:200)]
    public string Desc { get; set; }
    [Required]
    [StringLength(maximumLength:20)]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength:15)]
    public string AcademicDegree { get; set; }
    [Required]
    [StringLength(maximumLength:20)]
    public string Faculty { get; set; }
    [Required]
    public int Order { get; set; }
}