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
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = Constants.RoleConstants.Admin + " , " + Constants.RoleConstants.Moderator)]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public TestimonialController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var testimonials = await  _context.Testimonials.ToListAsync();
            return View(testimonials);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var testimonial = await  _context.Testimonials.FindAsync(id);
            if(testimonial == null) return NotFound();
            return View(testimonial);
        }

        public async Task<IActionResult> Update(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();
            return View(testimonial);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Testimonial testimonial)
        {
            bool isExist = await _context.Testimonials.AnyAsync(c => c.Id == id);
            if (!isExist) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }

            var testimonialToUpdate = await _context.Testimonials.FindAsync(id);
            if (testimonialToUpdate == null) return NotFound();

            if (testimonial.ImageFile != null)
            {
                FileUtils.DeleteFile(Path.Combine(_env.WebRootPath, FileConstants.ImagePath, FolderPath.Testimonial, testimonial.Image));
                var newImage = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Testimonial, testimonial.ImageFile);
                testimonialToUpdate.Image = newImage;
            }
        
        
            testimonialToUpdate.Name = testimonial.Name;
            testimonialToUpdate.Desc = testimonial.Desc;
            testimonialToUpdate.Faculty = testimonial.Faculty;
            testimonialToUpdate.AcademicDegree = testimonial.AcademicDegree;
            testimonialToUpdate.Order = testimonial.Order;
        
            _context.Testimonials.Update(testimonialToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public  IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialVM testimonial)
        {
            
            if(!ModelState.IsValid) return View();
            
            if (!testimonial.ImageFile.IsSupportedFile("image"))
            {
                ModelState.AddModelError(nameof(testimonial.ImageFile), "Image type required");
                return View();
            };
        
        
            if(testimonial.ImageFile.IsGreaterThanGivenMb(2))
            {
                ModelState.AddModelError(nameof(testimonial.ImageFile), "Maximum size is 2MB");
                return View();
            }

            var newTestimonial = new Testimonial
            {
                Name = testimonial.Name,
                Desc = testimonial.Desc,
                Faculty = testimonial.Faculty,
                AcademicDegree = testimonial.AcademicDegree,
                Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Testimonial , testimonial.ImageFile),
            };

            await _context.Testimonials.AddAsync(newTestimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        
        public async Task<IActionResult> Delete(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if(testimonial == null) return NotFound();
            return View(testimonial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if(testimonial == null) return NotFound();
            FileUtils.DeleteFile(Path.Combine(_env.WebRootPath, FileConstants.ImagePath, FolderPath.Testimonial, testimonial.Image));
            
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }   
}