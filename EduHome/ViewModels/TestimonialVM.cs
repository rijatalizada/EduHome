using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class TestimonialVM
{
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
    public int? Order { get; set; }
    
    public IFormFile ImageFile { get; set; }
}