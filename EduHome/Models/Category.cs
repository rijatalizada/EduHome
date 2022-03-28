using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Category
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please Enter Category Name")]
    [StringLength(maximumLength:30,MinimumLength =3,ErrorMessage = "maximum length is 30 and minimum length is 3")]
    public string Name { get; set; }
    [Required]
    public int Count { get; set; }
    public List<CourseCategory> CourseCategories { get; set; }
    public List<TeacherCategory> TeacherCategories { get; set; }
    public List<EventCategory> EventCategories { get; set; }
    public List<BlogCategory> BlogCategories { get; set; }
}