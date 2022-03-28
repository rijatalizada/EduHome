using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents;

public class NoticeBoardViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public NoticeBoardViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var homeNoticePage = await _context.HomeNoticePages.ToListAsync();

        return View(homeNoticePage);
    }
}