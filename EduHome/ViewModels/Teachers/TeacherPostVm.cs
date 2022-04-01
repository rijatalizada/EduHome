using System.ComponentModel.DataAnnotations;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewModels.Teachers;

public class TeacherPostVm
{
    //post
    [HiddenInput]
    public int Id { get; set; }
    public IFormFile? ImageFile { get; set; }
    [Required]
    [StringLength(maximumLength:30)]
    public string Name { get; set; }
    [Required]
    public string AcademicRanking { get; set; }
    [Required]
    public string About { get; set; }
    [Required]
    public string Experience { get; set; }
    [Required]
    public string Hobbies { get; set; }
    [Required]
    public string Mail { get; set; }
    [Required]
    
    public string Phone { get; set; }
    [Required]
    public string Skype { get; set; }
    public int? DegreeId { get; set; }
    public List<int> CategoryIds { get; set; }

    //get
    public TeacherSkills Skills { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Degree>?  Degrees{ get; set; }
}