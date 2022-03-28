using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class EventCategory
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int CategoryId { get; set; }
    public Event Event { get; set; }
    public Category Category { get; set; }
}