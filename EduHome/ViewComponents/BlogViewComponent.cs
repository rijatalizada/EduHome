using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents;

public class BlogViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public BlogViewComponent(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync(int count)
    {
        var blogs = await _context.Blogs.Take(count).ToListAsync();
        return View(blogs);
    }
}