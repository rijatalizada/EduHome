using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Assessment
{
    public int  Id { get; set; }
    [Required]
    
    public string Name { get; set; }
    public List<Course> Courses { get; set; }
}