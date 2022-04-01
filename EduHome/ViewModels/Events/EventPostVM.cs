using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewModels.Events;

public class EventPostVM
{
    //get
    [Required]
    [StringLength(maximumLength:50)]
    public string Name { get; set; }
    [Required]
    public string Time { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required] 
    public string Venue { get; set; }
    [Required]
    public string About { get; set; }
    [Required]
    public IFormFile? ImageFile { get; set; }
    public List<int> CategoryIds { get; set; }
    
    //post
    public List<Category>? Categories { get; set; }

}