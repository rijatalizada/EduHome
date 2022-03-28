using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class CourseController : Controller
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        Course courses = await _context.Courses.Include(c => c.CourseCategories).
            ThenInclude(cc => cc.Category)
            .Include(c => c.CourseLanguages).
            ThenInclude(cl => cl.Language).Include(c=>c.Assessment).
            Include(c=>c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
        return View(courses);
    }
}