using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EduHome.Areas.Dashboard.Controllers;


[Area("Dashboard")]
public class NoticeBoardController : Controller
{
    private readonly AppDbContext _context;

    public NoticeBoardController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    { 
        var noticeBoards = await _context.HomeNoticePages.ToListAsync();
        return View(noticeBoards);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        var noticeBoard = await _context.HomeNoticePages.FindAsync(id);
        if (noticeBoard == null)  return NotFound();
        

        return View(noticeBoard);
    }
    
    public async Task<IActionResult> Create()
    {
        
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NoticeBoardVM noticeBoard)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        var newNoticePost = new HomeNoticePage
        {
            Description = noticeBoard.Description,
            PostDate = DateTime.Now,
           
        };
        
        await _context.HomeNoticePages.AddAsync(newNoticePost);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var noticeBoard = await _context.HomeNoticePages.FindAsync(id);
        if(noticeBoard == null) return NotFound();
        return View(noticeBoard);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, NoticeBoardVM noticeBoard)
    {
        bool isExist = await _context.HomeNoticePages.AnyAsync(c => c.Id == id);
        if (!isExist) return NotFound();
        if (!ModelState.IsValid) return View();
        
        var updatedNoticePost = await _context.HomeNoticePages.FindAsync(id);
        if(updatedNoticePost == null) return NotFound();
        updatedNoticePost.Description = noticeBoard.Description;
        updatedNoticePost.PostDate = DateTime.Now;

        _context.HomeNoticePages.Update(updatedNoticePost);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    
    public async Task<IActionResult> Delete(int id)
    {
        var noticeBoard = await _context.HomeNoticePages.FindAsync(id);
        if (noticeBoard == null) return NotFound();
        return View(noticeBoard);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, HomeNoticePage noticePost)
    {
        var noticePostToDelete = await _context.HomeNoticePages.FindAsync(id);
        if (noticePostToDelete == null) return NotFound();
        _context.HomeNoticePages.Remove(noticePostToDelete);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
