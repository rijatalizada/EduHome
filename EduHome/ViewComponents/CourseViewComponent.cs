using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents;

public class CourseViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public CourseViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count)
    {
        List<Course> course = await _context.Courses.Take(count).ToListAsync();
        return View(course);
    }
}