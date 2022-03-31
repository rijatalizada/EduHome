namespace EduHome.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Deleted { get; set; }
    public bool IsActive { get; set; }
}