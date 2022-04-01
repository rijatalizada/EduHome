using System.ComponentModel.DataAnnotations;
using EduHome.Models;

namespace EduHome.Areas.Dashboard.ViewModels.Courses;

public class CoursePostVM
{
    //get 
    [Required]
    [StringLength(maximumLength:30)]
    public string Name  { get; set; }
    public IFormFile? ImageFile { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string About { get; set; }
    [Required]
    public string HowToApply { get; set; }
    [Required]
    public string Certification { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public int Duration { get; set; }
    [Required]
    public int ClassDuration { get; set; }
    [Required]
    public int Fee { get; set; }
    [Required]
    public int Students { get; set; }
    public int AssessmentId { get; set; }
    public int SkillLevelId { get; set; }
    [Required]
    public List<int> CategoryIds { get; set; }
    public List<int> LanguageIds { get; set; }
    
    //get 
    public List<Category>? Categories { get; set; }
    public List<Language>? Languages { get; set; }
    public List<Assessment>? Assessments { get; set; }
    public List<SkillLevel>? SkillLevels { get; set; }
    
    
    
}