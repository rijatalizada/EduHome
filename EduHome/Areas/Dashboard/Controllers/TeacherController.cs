using EduHome.Constants;
using EduHome.DAL;
using EduHome.Extensions;
using EduHome.ImageFolder;
using EduHome.Models;
using EduHome.Utils;
using EduHome.ViewModels.Teachers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace EduHome.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class TeacherController : Controller
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers.Select(t => new TeacherIndexVM
        {
            Id = t.Id,
            Name = t.Name,
            Image = t.Image,
            AcademicRanking = t.AcademicRanking
        }).ToListAsync();
        return View(teachers);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var teacher = await  _context.Teachers.Include(tc => tc.TeacherCategories)
            .ThenInclude(c=>c.Category).Include(td=>td.Degree)
            .Include(ts => ts.TeacherSkills).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (teacher == null) return NotFound();
        
        return View(teacher);
    }

    public async Task<IActionResult> Create()
    {
        TeacherPostVm model = new TeacherPostVm
        {
            Degrees = await _context.Degrees.ToListAsync(),
            Categories = await _context.Categories.ToListAsync(),
        };
       
        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(TeacherPostVm model)
    {
        model.Degrees = await _context.Degrees.ToListAsync();
        model.Categories = await _context.Categories.ToListAsync();
        if (!ModelState.IsValid) return View(model);
        
        
        var degree = await _context.Degrees.FindAsync(model.DegreeId);
        if (degree == null)
        {
            ModelState.AddModelError(nameof(TeacherPostVm.DegreeId), "Degree Not Found");
            return View(model);
        }
        
        
        List<TeacherCategory> teacherCategories = new List<TeacherCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                ModelState.AddModelError(nameof(TeacherPostVm.CategoryIds), "Category Not Found");
                return View(model);
            }

            teacherCategories.Add(new TeacherCategory { CategoryId = categoryId });
        }

        TeacherSkills skills = new TeacherSkills
        {
            Communication = model.Skills.Communication,
            Language = model.Skills.Language,
            Design = model.Skills.Design,
            Development = model.Skills.Development,
            Innovation = model.Skills.Innovation,
            TeamLeader = model.Skills.TeamLeader,
            TeacherId = model.Id
        };
        
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

        Teacher teacher = new Teacher
        {
            Id = model.Id,
            Name = model.Name,
            AcademicRanking = model.AcademicRanking,
            Mail = model.Mail,
            Phone = model.Phone,
            Skype = model.Skype,
            DegreeId = model.DegreeId,
            Hobbies = model.Hobbies,
            Experience = model.Experience,
            About = model.About,
            Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Teacher ,model.ImageFile),
            TeacherCategories = teacherCategories,
            TeacherSkills = skills
        };
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var teacher = await _context.Teachers.Include(tc => tc.TeacherCategories)
            .ThenInclude(c => c.Category).Include(td => td.Degree)
            .Include(ts => ts.TeacherSkills).FirstOrDefaultAsync(t => t.Id == id);
        
        if (teacher == null) return NotFound();

        TeacherPostVm model = new TeacherPostVm
        {
            Name = teacher.Name,
            AcademicRanking = teacher.AcademicRanking,
            Mail = teacher.Mail,
            Phone = teacher.Phone,
            Skype = teacher.Skype,
            Hobbies = teacher.Hobbies,
            Experience = teacher.Experience,
            About = teacher.About,
            DegreeId = teacher.DegreeId,
            CategoryIds =  teacher.TeacherCategories.Select(c => c.CategoryId).ToList(),
            Skills = teacher.TeacherSkills,
            Categories =  _context.Categories.ToList(),
            Degrees =  _context.Degrees.ToList(),
        };
        
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, TeacherPostVm model)
    {
        var teacher = await _context.Teachers.Include(tc => tc.TeacherCategories)
            .ThenInclude(c => c.Category).Include(td => td.Degree)
            .Include(ts => ts.TeacherSkills).FirstOrDefaultAsync(t => t.Id == id);
        if(teacher==null) return NotFound();
        
        model.Categories = await  _context.Categories.ToListAsync();
        model.Degrees = await _context.Degrees.ToListAsync();
        
        if (!ModelState.IsValid) return View(model);
        
        var degree = await _context.Degrees.FindAsync(model.DegreeId);
        if (degree == null)
        {
            ModelState.AddModelError(nameof(TeacherPostVm.DegreeId), "Degree Not Found");
            return View(model);
        }
        
        List<TeacherCategory> teacherCategories = new List<TeacherCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                ModelState.AddModelError(nameof(TeacherPostVm.CategoryIds), "Category Not Found");
                return View(model);
            }

            teacherCategories.Add(new TeacherCategory { CategoryId = categoryId });
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
            FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Teacher, teacher.Image));
            
        }
        
        TeacherSkills skills = new TeacherSkills
        {
            Communication = model.Skills.Communication,
            Language = model.Skills.Language,
            Design = model.Skills.Design,
            Development = model.Skills.Development,
            Innovation = model.Skills.Innovation,
            TeamLeader = model.Skills.TeamLeader,
            TeacherId = model.Id
        };

        teacher.Name = model.Name;
        teacher.AcademicRanking = model.AcademicRanking;
        teacher.Mail = model.Mail;
        teacher.Phone = model.Phone;
        teacher.Skype = model.Skype;
        teacher.Hobbies = model.Hobbies;
        teacher.Experience = model.Experience;
        teacher.About = model.About;
        teacher.DegreeId = model.DegreeId;
        teacher.Image = model.ImageFile != null ? FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Teacher, model.ImageFile) : teacher.Image;
        teacher.TeacherCategories = teacherCategories;
        teacher.TeacherSkills = skills;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.Include(tc => tc.TeacherCategories)
            .ThenInclude(c => c.Category).Include(td => td.Degree)
            .Include(ts => ts.TeacherSkills).FirstOrDefaultAsync(t => t.Id == id);
        
        if(teacher==null) return NotFound();
        
        return View(teacher);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var teacher = await _context.Teachers.Include(tc => tc.TeacherCategories)
            .ThenInclude(c => c.Category).Include(td => td.Degree)
            .Include(ts => ts.TeacherSkills).AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if(teacher==null) return NotFound();
        
        FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Teacher, teacher.Image));

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}