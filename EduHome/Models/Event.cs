using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Event
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength:50)]
    public string Name { get; set; }
    public string Image { get; set; }
    [Required]
    public string Time { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required] 
    public string Venue { get; set; }
    [Required]
    public string About { get; set; }

    public List<EventCategory> EventCategories { get; set; }
    public List<Comment> Comments { get; set; }

}