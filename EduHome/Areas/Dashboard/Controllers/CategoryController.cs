using EduHome.Constants;
using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.Moderator)]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        var category = await _context.Categories.Include(c=>c.CourseCategories).
            ThenInclude(cc=>cc.Course).
            Include(c=>c.EventCategories).ThenInclude(ec=>ec.Event)
            .FirstOrDefaultAsync(c=>c.Id == id);
        return View(category);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryVM category)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        var newCategory = new Category
        {
            Name = category.Name,
            Count = category.Count,
          
        };
        await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, CategoryVM category)
    {
        bool isExist = await _context.Categories.AnyAsync(c => c.Id == id);
        if (!isExist) return NotFound();
        if (!ModelState.IsValid) return View();
        
        var updateCategory = await _context.Categories.FindAsync(id);
        if(updateCategory == null) return NotFound();
        updateCategory.Name = category.Name;
        updateCategory.Count = category.Count;

        _context.Categories.Update(updateCategory);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.Include(c=>c.CourseCategories).
            ThenInclude(cc=>cc.Course).
            Include(c=>c.EventCategories).ThenInclude(ec=>ec.Event)
            .FirstOrDefaultAsync(c=>c.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, Category category)
    {
        var categoryToDelete = await _context.Categories.Include(c=>c.CourseCategories).
            ThenInclude(cc=>cc.Course).
            Include(c=>c.EventCategories).ThenInclude(ec=>ec.Event)
            .FirstOrDefaultAsync(c=>c.Id == id);
        if (categoryToDelete == null) return NotFound();
        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}