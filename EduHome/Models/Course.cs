using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace EduHome.Models;

public class Course
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength:30)]
    public string Name  { get; set; }
    
    public string Image { get; set; }
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
    public int? ClassDuration { get; set; }
    [Required]
    public int? Fee { get; set; }
    [Required]
    public int Students { get; set; }
    public int SkillLevelId { get; set; }
    public SkillLevel SkillLevel { get; set; }
    public int AssessmentId { get; set; }
    public Assessment Assessment { get; set; }
    public List<CourseCategory> CourseCategories { get; set; }
    public List<CourseLanguage> CourseLanguages { get; set; }

}