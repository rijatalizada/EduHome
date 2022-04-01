using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.Dashboard.ViewModels.Courses;

public class CourseListVM
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength:30)]
    public string Name  { get; set; }
    
    public string Image { get; set; }
}