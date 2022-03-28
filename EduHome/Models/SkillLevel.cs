namespace EduHome.Models;

public class SkillLevel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Course> Courses { get; set; }
}