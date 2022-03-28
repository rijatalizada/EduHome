using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Language
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public List<CourseLanguage> CourseLanguages { get; set; }
}