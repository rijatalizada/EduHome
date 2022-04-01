using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduHome.Constants;
using EduHome.DAL;
using EduHome.Areas.Dashboard.ViewModels.Courses;
using EduHome.Extensions;
using EduHome.ImageFolder;
using EduHome.Models;
using EduHome.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.Moderator)]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.Select(c => new CourseListVM
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
            }).ToListAsync();
            return View(courses);
        }
        
        public async Task<IActionResult> DetailAdmin(int id)
        {
            var course = await  _context.Courses.Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.CourseLanguages).ThenInclude(cl => cl.Language).Include(c => c.Assessment)
                .Include(c => c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
        
            return View(course);
        }
        
        public async Task<IActionResult> Create()
        {
            
            CoursePostVM model = new CoursePostVM
            {
                Assessments = await _context.Assessments.ToListAsync(),
                SkillLevels = await _context.SkillLevels.ToListAsync(),
                Languages = await _context.Languages.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
            };
       
            return View(model);
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoursePostVM model)
        { 
            
            model.Assessments = await _context.Assessments.ToListAsync();
        model.SkillLevels = await _context.SkillLevels.ToListAsync();
        model.Categories = await _context.Categories.ToListAsync();
        model.Languages = await _context.Languages.ToListAsync();
        if (!ModelState.IsValid) return View(model);
        
        
        var assessment = await _context.Assessments.FindAsync(model.AssessmentId);
        if (assessment == null)
        {
            ModelState.AddModelError(nameof(CoursePostVM.AssessmentId), "Assessment Not Found");
            return View(model);
        }
        
        var skillLevel = await _context.SkillLevels.FindAsync(model.SkillLevelId);
        if (skillLevel == null)
        {
            ModelState.AddModelError(nameof(CoursePostVM.SkillLevelId), "Skill Level Not Found");
            return View(model);
        }
        
        
        List<CourseCategory> courseCategories = new List<CourseCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                ModelState.AddModelError(nameof(CoursePostVM.CategoryIds), "Category Not Found");
                return View(model);
            }

            courseCategories.Add(new CourseCategory { CategoryId = categoryId });
        }
        
        List<CourseLanguage> courseLanguages = new List<CourseLanguage>();
        foreach (var languageId in model.LanguageIds)
        {
            var language = await _context.Languages.FindAsync(languageId);
            if (language == null)
            {
                ModelState.AddModelError(nameof(CoursePostVM.LanguageIds), "Language Not Found");
                return View(model);
            }

            courseLanguages.Add(new CourseLanguage { LanguageId = languageId });
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

        Course course = new Course
        {
            Name = model.Name,  
            About = model.About,
            Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Course ,model.ImageFile),
            AssessmentId = model.AssessmentId,
            HowToApply = model.HowToApply,
            SkillLevelId = model.SkillLevelId,
            CourseCategories = courseCategories,
            CourseLanguages = courseLanguages,
            StartDate = model.StartDate,
            Duration = model.Duration,
            Certification = model.Certification,
            Description = model.Description,
            ClassDuration = model.ClassDuration,
            Fee = model.Fee,
            Students = model.Students
        };
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
        
        
        public async Task<IActionResult> Update(int id)
        {
            var course = await  _context.Courses.Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.CourseLanguages).ThenInclude(cl => cl.Language).Include(c => c.Assessment)
                .Include(c => c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();


            CoursePostVM model = new CoursePostVM
            {
                Name = course.Name,  
                About = course.About,
                AssessmentId = course.AssessmentId,
                HowToApply = course.HowToApply,
                SkillLevelId = course.SkillLevelId,
                StartDate = course.StartDate,
                Duration = course.Duration,
                Certification = course.Certification,
                Description = course.Description,
                ClassDuration = course.ClassDuration,
                Fee = course.Fee,
                Students = course.Students,
                CategoryIds =  course.CourseCategories.Select(c => c.CategoryId).ToList(),
                LanguageIds = course.CourseLanguages.Select(c => c.LanguageId).ToList(),
                Categories =  _context.Categories.ToList(),
                Languages =  _context.Languages.ToList(),
                Assessments = _context.Assessments.ToList(),
                SkillLevels = _context.SkillLevels.ToList(),
            };
        
            return View(model);
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CoursePostVM model)
        {
        var course = await  _context.Courses.Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
            .Include(c => c.CourseLanguages).ThenInclude(cl => cl.Language).Include(c => c.Assessment)
            .Include(c => c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
        if (course == null) return NotFound();

        model.Categories = await  _context.Categories.ToListAsync();
        model.Languages = await _context.Languages.ToListAsync();
        
        if (!ModelState.IsValid) return View(model);
        
        var assessment = await _context.Assessments.FindAsync(model.AssessmentId);
        if (assessment == null)
        {
            ModelState.AddModelError(nameof(CoursePostVM.AssessmentId), "Assessment Not Found");
            return View(model);
        }
        
        var skillLevel = await _context.SkillLevels.FindAsync(model.SkillLevelId);
        if (skillLevel == null)
        {
            ModelState.AddModelError(nameof(CoursePostVM.SkillLevelId), "Skill Level Not Found");
            return View(model);
        }
        
        
        List<CourseCategory> courseCategories = new List<CourseCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                ModelState.AddModelError(nameof(CoursePostVM.CategoryIds), "Category Not Found");
                return View(model);
            }

            courseCategories.Add(new CourseCategory { CategoryId = categoryId });
        }
        
        List<CourseLanguage> courseLanguages = new List<CourseLanguage>();
        foreach (var languageId in model.LanguageIds)
        {
            var language = await _context.Languages.FindAsync(languageId);
            if (language == null)
            {
                ModelState.AddModelError(nameof(CoursePostVM.LanguageIds), "Language Not Found");
                return View(model);
            }

            courseLanguages.Add(new CourseLanguage { LanguageId = languageId });
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
            FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Course, course.Image));
            
        }

        course.Name = model.Name;
        course.Description = model.Description;
        course.About = model.About;
        course.HowToApply = model.HowToApply;
        course.Certification = model.Certification;
        course.Duration = model.Duration;
        course.ClassDuration = model.ClassDuration;
        course.AssessmentId = model.AssessmentId;
        course.SkillLevelId = model.SkillLevelId;
        course.Students = model.Students;
        course.Fee = model.Fee;
        course.StartDate = model.StartDate;
        course.Image = model.ImageFile != null ? FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Course, model.ImageFile) : course.Image;
        course.CourseCategories = courseCategories;
        course.CourseLanguages = courseLanguages;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
        
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var course = await  _context.Courses.Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.CourseLanguages).ThenInclude(cl => cl.Language).Include(c => c.Assessment)
                .Include(c => c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
        
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var course = await  _context.Courses.Include(c => c.CourseCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.CourseLanguages).ThenInclude(cl => cl.Language).Include(c => c.Assessment)
                .Include(c => c.SkillLevel).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
        
            FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Course, course.Image));

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        
    }
}