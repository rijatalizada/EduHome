using System.Diagnostics;
using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// using EduHome.Models;x

namespace EduHome.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM model = new HomeVM()
        {
            HomeSlider = await _context.HeaderSliders.OrderBy(hs=>hs.Order).ToListAsync(),
            Event = await _context.Events.Include(e => e.EventCategories).ThenInclude(ec => ec.Category).Take(8)
                .ToListAsync(),
        };
        return View(model);
    }

    public async Task<IActionResult> About()
    {
        return View();
    }

    
}