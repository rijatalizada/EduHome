using EduHome.Constants;
using EduHome.DAL;
using EduHome.Extensions;
using EduHome.ImageFolder;
using EduHome.Models;
using EduHome.Utils;
using EduHome.Areas.Dashboard.ViewModels.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = RoleConstants.Admin + "," + RoleConstants.Moderator)]
public class BlogController : Controller
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }
        
    public async Task<IActionResult> Index()
    {
        var blogs = await _context.Blogs.ToListAsync();
        return View(blogs);
    }
    
    public async Task<IActionResult> Detail(int id)
    {
        var blog = await _context.Blogs.
            Include(e=>e.BlogCategories).
            ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);
        if(blog == null)
        {
            return NotFound();
        }

        return View(blog);
    }

    public async Task<IActionResult> Create()
    {
        BlogPostVm model = new BlogPostVm()
        {
            Categories = await _context.Categories.ToListAsync()
        };

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPostVm model)
    {
        model.Categories = await _context.Categories.ToListAsync();
        if (!ModelState.IsValid) return View(model);
            
        List<BlogCategory> blogCategories = new List<BlogCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(BlogPostVm.Categories), "Category Not Found");
                return View(model);
            }
            blogCategories.Add(new BlogCategory()
            {
                Category = category
            });
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

        Blog newBlog = new Blog
        {
            Name = model.Name,
            About = model.About,
            Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Blog, model.ImageFile),
            Date = model.Date,
            Author = model.Author,
            BlogCategories = blogCategories
        };
            
        await _context.Blogs.AddAsync(newBlog);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

    }
    
    public async Task<IActionResult> Update(int id)
    {
        var model = await _context.Blogs.Include(e=>e.BlogCategories).
            ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);
        if(model == null)
        {
            return NotFound();
        }

        BlogPostVm updateBlog = new BlogPostVm
        {
            Name = model.Name,
            About = model.About,
            Date = model.Date,
            Author = model.Author,
            CategoryIds = model.BlogCategories.Select(c=>c.CategoryId).ToList(),
            Categories =  _context.Categories.ToList(),
        };
        
        return View(updateBlog);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, BlogPostVm model)
    {
        var blog = await _context.Blogs.Include(e=>e.BlogCategories).
            ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(t => t.Id == id);
        if(blog==null) return NotFound();
        
        model.Categories = await  _context.Categories.ToListAsync();
        if (!ModelState.IsValid) return View(model);
        
        List<BlogCategory> blogCategories = new List<BlogCategory>();
        foreach (var categoryId in model.CategoryIds)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                ModelState.AddModelError(nameof(BlogPostVm.CategoryIds), "Category Not Found");
                return View(model);
            }

            blogCategories.Add(new BlogCategory { CategoryId = categoryId });
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
            FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Blog, blog.Image));
            
        }
        
        blog.Name = model.Name; 
        blog.About = model.About;
        blog.Date = model.Date;
        blog.Author = model.Author;
        blog.BlogCategories = blogCategories;
        if (model.ImageFile != null) 
        {
            blog.Image = FileUtils.CreateFile(FileConstants.ImagePath, FolderPath.Blog, model.ImageFile);
        }
            

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var blog  = await _context.Blogs.
            Include(e=>e.BlogCategories).
            ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);

        if (blog == null) return NotFound();

        return View(blog);
    }
        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, BlogPostVm model)
    {
        var blog  = await _context.Blogs.
            Include(e=>e.BlogCategories).
            ThenInclude(ec=>ec.Category).FirstOrDefaultAsync(e => e.Id == id);

        if (blog == null) return NotFound();
            
        FileUtils.DeleteFile(Path.Combine(FileConstants.ImagePath, FolderPath.Blog, blog.Image));
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}