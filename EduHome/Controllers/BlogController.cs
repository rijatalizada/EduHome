using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class BlogController : Controller
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        Blog blog = _context.Blogs.Include(b=>b.BlogCategories).ThenInclude(bc=>bc.Category)
            .FirstOrDefault(b => b.Id == id);
        return View(blog);
    }
}