namespace EduHome.Models;

public class TeacherCategory
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int CategoryId { get; set; }
    public Teacher Teacher { get; set; }
    public Category Category { get; set; }
}