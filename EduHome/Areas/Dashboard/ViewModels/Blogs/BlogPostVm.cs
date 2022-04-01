using System.ComponentModel.DataAnnotations;
using EduHome.Models;

namespace EduHome.Areas.Dashboard.ViewModels.Blogs;

public class BlogPostVm
{
    //get
    [Required]
    [StringLength(maximumLength:100)]
    public string Name { get; set; }
    public IFormFile? ImageFile { get; set; }
    [Required]
    public string About { get; set; }
    [Required]
    [StringLength(maximumLength:25)]
    public string Author { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public List<int> CategoryIds { get; set; }
    
    //set
    public List<Category>? Categories { get; set; }
}