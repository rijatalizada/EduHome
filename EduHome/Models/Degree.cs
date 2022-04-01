using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome.Models;

public class Degree
{
    public int Id { get; set; }
    [Required]
    [Column("Degree")]
    public string Name { get; set; }
    public List<Teacher> Teachers { get; set; }
   
}