using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Teacher
{
    public int Id { get; set; }
    public string Image { get; set; }
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
    public Degree Degree { get; set; }
    public TeacherSkills TeacherSkills { get; set; }
    public List<TeacherCategory> TeacherCategories { get; set; }
    
}