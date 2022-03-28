using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Blog
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength:100)]
    public string Name { get; set; }
    public string Image { get; set; }
    [Required]
    public string About { get; set; }
    [Required]
    [StringLength(maximumLength:25)]
    public string Author { get; set; }
    [Required]
    public DateTime Date { get; set; }

    public List<BlogCategory> BlogCategories { get; set; }
}