using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class TeacherController : Controller
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        Teacher teacher = await  _context.Teachers.Include(tc => tc.TeacherCategories)
            .ThenInclude(c=>c.Category).Include(td=>td.Degree)
            .Include(ts => ts.TeacherSkills).FirstOrDefaultAsync(t => t.Id == id);
        return View(teacher);
    }
}