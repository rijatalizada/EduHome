using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TestimonialViewComponent : ViewComponent
{
    private readonly AppDbContext _context;
    
    public TestimonialViewComponent(AppDbContext context)
    {
        _context = context;
    }   
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var homeFooterSlider = await _context.Testimonials.OrderBy(hfs=>hfs.Order).ToListAsync();
        return View(homeFooterSlider);
    }   
}