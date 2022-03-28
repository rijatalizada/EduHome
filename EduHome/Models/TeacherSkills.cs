namespace EduHome.Models;

public class TeacherSkills
{
    public int  Id { get; set; }
    public int Language { get; set; }
    public int TeamLeader { get; set; }
    public int Development { get; set; }
    public int Innovation { get; set; }
    public int Communication { get; set; }
    public int Design { get; set; }
    public int TeacherId { get; set; }
    
    public Teacher Teacher { get; set; }
}