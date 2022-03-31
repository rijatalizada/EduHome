using EduHome.Areas.Dashboard.ViewModels;
using EduHome.Constants;
using EduHome.DAL;
using EduHome.Extensions;
using EduHome.ImageFolder;
using EduHome.Models;
using EduHome.Utils;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.Moderator)]

public class HomeHeaderSliderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public HomeHeaderSliderController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async  Task<IActionResult> Index()
    {
        var homeHeaderSlider = await _context.HeaderSliders.ToListAsync();
        return View(homeHeaderSlider);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        var slider = await _context.HeaderSliders.FindAsync(id);
        return View(slider);
    }

    public IActionResult Update(int id)
    {
        var slider = _context.HeaderSliders.Find(id);
        return View(slider);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, HeaderSlider slider)
    {
        bool isExist = await _context.HeaderSliders.AnyAsync(c => c.Id == id);
        if (!isExist) return NotFound();
        if (!ModelState.IsValid)
        {
            return View();
        }

        var sliderToUpdate = await _context.HeaderSliders.FindAsync(id);
        if (sliderToUpdate == null) return NotFound();

        if (slider.ImageFile != null)
        {
            FileUtils.DeleteFile(Path.Combine(_env.WebRootPath, FileConstants.ImagePath, FolderPath.Slider, sliderToUpdate.Image));
            var newImage = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Slider, slider.ImageFile);
            sliderToUpdate.Image = newImage;
        }
        
        
        sliderToUpdate.Title = slider.Title;
        sliderToUpdate.TitleH2 = slider.TitleH2;
        sliderToUpdate.Description = slider.Description;
        sliderToUpdate.Order = slider.Order;
        
        _context.HeaderSliders.Update(sliderToUpdate);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HeaderSliderVM slider)
    { 
        if(!ModelState.IsValid) return View();

        if (!slider.ImageFile.IsSupportedFile("image"))
        {
            ModelState.AddModelError(nameof(slider.ImageFile), "Image type required");
            return View();
        };
        
        
        if(slider.ImageFile.IsGreaterThanGivenMb(2))
        {
            ModelState.AddModelError(nameof(slider.ImageFile), "Maximum size is 2MB");
            return View();
        }

        var newSlider = new HeaderSlider
        {
            Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Slider , slider.ImageFile),
            Title = slider.Title,
            TitleH2 = slider.TitleH2,
            Order = slider.Order,
            Description = slider.Description
        };

        await _context.HeaderSliders.AddAsync(newSlider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    
    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _context.HeaderSliders.FindAsync(id);
        if (slider == null) return NotFound();
        return View(slider);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteSlider(int id)
    {
        var slider = await _context.HeaderSliders.FindAsync(id);
        if (slider == null) return NotFound();
        FileUtils.DeleteFile(Path.Combine(_env.WebRootPath, FileConstants.ImagePath, FolderPath.Slider, slider.Image));

        _context.HeaderSliders.Remove(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public  IActionResult CreateMultiple()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMultiple(HeaderMultipleSlidersVM model)
    {
        if(!ModelState.IsValid) return View();
        byte order = 4;
        foreach (var image in model.Images)
        {
            if (!image.IsSupportedFile("image"))
            {
                ModelState.AddModelError(nameof(model.Images), "Image type required");
                return View();
            };
            
            if (image.IsGreaterThanGivenMb(2))
            {
                ModelState.AddModelError(nameof(model.Images), "Maximum size is 2MB");
                return View();
            }

            HeaderSlider slider = new HeaderSlider
            {
                Description = model.Description,
                Title = model.Title,
                TitleH2 = model.TitleH2,
                Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Slider, image),
                Order = order++
            };
            
            await _context.HeaderSliders.AddAsync(slider);
           
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}