namespace EduHome.Models;

public class CourseLanguage
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int LanguageId { get; set; }
    public Course Course { get; set; }
    public Language Language { get; set; }
}