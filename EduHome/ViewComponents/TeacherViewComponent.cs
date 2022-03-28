using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents;

public class TeacherViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public TeacherViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count)
    {
        
        List<Teacher> teacher = await _context.Teachers.Take(count).ToListAsync();
        return View(teacher);
    }
}