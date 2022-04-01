namespace EduHome.Models;

public class Comment : BaseEntity
{
    public string Description { get; set; }
    public string Subject { get; set; }
    public string? UserId { get; set; }
    public User User { get; set; }
    public int? CourseId { get; set; }
    public Course Course { get; set; }
    public int? BlogId { get; set; }
    public Blog Blog { get; set; }
    public int? EventId { get; set; }
    public Event Event { get; set; }
    
}