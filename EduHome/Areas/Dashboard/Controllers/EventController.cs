using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduHome.Constants;
using EduHome.DAL;
using EduHome.Extensions;
using EduHome.ImageFolder;
using EduHome.Models;
using EduHome.Utils;
using EduHome.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.Moderator)]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = await _context.Events.
                Include(e=>e.EventCategories).
                ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);
            if(model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            EventPostVM model = new EventPostVM
            {
                Categories = await _context.Categories.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventPostVM model)
        {
            model.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid) return View(model);
            
            List<EventCategory> eventCategories = new List<EventCategory>();
            foreach (var categoryId in model.CategoryIds)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    ModelState.AddModelError(nameof(EventPostVM.Categories), "Category Not Found");
                    return View(model);
                }
                eventCategories.Add(new EventCategory { Category = category });
            }
            
            if (!model.ImageFile.IsSupportedFile("image"))
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Image type required");
                return View();
            };
        
        
            if(model.ImageFile.IsGreaterThanGivenMb(2))
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Maximum size is 2MB");
                return View();
            }

            Event newEvent = new Event
            {
                Name = model.Name,
                About = model.About,
                Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Event, model.ImageFile),
                Date = model.Date,
                Time = model.Time,
                Venue = model.Venue,
                EventCategories = eventCategories
            };
            
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _context.Events.Include(e=>e.EventCategories).
                ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);
            if(model == null)
            {
                return NotFound();
            }

            EventPostVM updateEvent = new EventPostVM
            {
                Name = model.Name,
                About = model.About,
                Date = model.Date,
                Time = model.Time,
                Venue = model.Venue,
                CategoryIds = model.EventCategories.Select(c=>c.CategoryId).ToList(),
                Categories =  _context.Categories.ToList(),
            };
        
            return View(updateEvent);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, EventPostVM model)
        {
        var eventModel = await _context.Events.Include(e=>e.EventCategories).
            ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(t => t.Id == id);
        if(eventModel==null) return NotFound();
        
        model.Categories = await  _context.Categories.ToListAsync();
        if (!ModelState.IsValid) return View(model);
        
        List<EventCategory> eventCategories = new List<EventCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                ModelState.AddModelError(nameof(EventPostVM.CategoryIds), "Category Not Found");
                return View(model);
            }

            eventCategories.Add(new EventCategory { CategoryId = categoryId });
        }

        if (model.ImageFile != null)
        {
            if (!model.ImageFile.IsSupportedFile("image"))
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Image type required");
                return View();
            };
            if(model.ImageFile.IsGreaterThanGivenMb(2))
            {
                ModelState.AddModelError(nameof(model.ImageFile), "Maximum size is 2MB");
                return View();
            }
            FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Event, eventModel.Image));
            
        }
        
            eventModel.Name = model.Name;
            eventModel.About = model.About;
            eventModel.Date = model.Date;
            eventModel.Time = model.Time;
            eventModel.Venue = model.Venue;
            eventModel.EventCategories = eventCategories;
            if (model.ImageFile != null)
            {
                eventModel.Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Event, model.ImageFile);
            }
            

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var eventModel  = await _context.Events.
                Include(e=>e.EventCategories).
                ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null) return NotFound();

            return View(eventModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, EventPostVM model)
        {
            var eventModel  = await _context.Events.
                Include(e=>e.EventCategories).
                ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null) return NotFound();
            
            FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Event, eventModel.Image));
            _context.Events.Remove(eventModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}