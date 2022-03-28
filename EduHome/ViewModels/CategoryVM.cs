using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class CategoryVM
{
    [Required(ErrorMessage = "Please Enter Category Name")]
    [StringLength(maximumLength:30,MinimumLength =3,ErrorMessage = "maximum length is 30 and minimum length is 3")]
    public string Name { get; set; }
    [Required]
    public int Count { get; set; }
}