using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels.Teachers;

public class TeacherIndexVM
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength:30)]
    public string Name { get; set; }
    public string Image { get; set; }
    [Required]
    public string AcademicRanking { get; set; }
}