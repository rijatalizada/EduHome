using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class CourseController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public CourseController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        CourseDetailVM model = new CourseDetailVM
        {
             Course = await _context.Courses.Include(c => c.CourseCategories).
            ThenInclude(cc => cc.Category)
            .Include(c => c.CourseLanguages).
            ThenInclude(cl => cl.Language).Include(c=>c.Assessment).
            Include(c=>c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id),
   

        };
        if (model.Course == null) return NotFound();
        
        
        return View(model);
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int id, CourseDetailVM model)
    {
        var course = await _context.Courses.Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
            .Include(c => c.CourseLanguages).ThenInclude(cl => cl.Language).Include(c => c.Assessment)
            .Include(c => c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
        if (course == null) return NotFound();

      

        var comment = new Comment
        {
            Description = model.Comment.Description,
            UserId = _userManager.GetUserId(User),
            CourseId = id,
        };
        
        _context.Comments.Add(comment); 
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Detail), new { id });
    }
    
    
}