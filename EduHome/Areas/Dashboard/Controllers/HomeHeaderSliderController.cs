using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class HomeHeaderSliderController : Controller
{
    private readonly AppDbContext _context;

    public HomeHeaderSliderController(AppDbContext context)
    {
        _context = context;
    }

    public async  Task<IActionResult> Index()
    {
        var homeHeaderSlider = await _context.HeaderSliders.ToListAsync();
        return View(homeHeaderSlider);
    }
    
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HeaderSlider slider)
    { 
        if(!ModelState.IsValid) return View();

        if (!slider.ImageFile.ContentType.Contains("image"))
        {
            ModelState.AddModelError(nameof(slider.ImageFile), "Image type required");
            return View();
        };
        
        if(slider.ImageFile.Length > (1024 * 1024 * 2))
        {
            ModelState.AddModelError(nameof(slider.ImageFile), "Maximum size is 2MB");
            return View();
        }
        
        return RedirectToAction(nameof(Index));
    }
    
}