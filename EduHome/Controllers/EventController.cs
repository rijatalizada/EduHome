using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class EventController : Controller
{
    private readonly AppDbContext _context;

    public EventController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Event> events = await _context.Events.Include(e=>e.EventCategories).ThenInclude(ec=>ec.Category)
            .ToListAsync();
        
        return View(events);
    }

    public async Task<IActionResult> Detail(int id, int categoryId)
    {
        Event _event = await _context.Events.Include(e=>e.EventCategories).
            ThenInclude(ec=>ec.Category).
            ThenInclude(c=>c.TeacherCategories).ThenInclude(tc=>tc.Teacher).ThenInclude(t=>t.Degree).
            FirstOrDefaultAsync(e => e.Id == id);

        ViewBag.Speakers = await _context.Teachers
            .Where(t => t.TeacherCategories.FirstOrDefault().CategoryId == categoryId).ToListAsync();
        
        return View(_event);
    }
}