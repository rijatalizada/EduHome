using EduHome.Models;
using Microsoft.EntityFrameworkCore;

namespace EduHome.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
    {
        
    }
    
    public DbSet<HeaderSlider> HeaderSliders { get; set; }
    public DbSet<HomeNoticePage> HomeNoticePages { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<CourseLanguage> CourseLanguages { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<SkillLevel> SkillLevels { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CourseCategory> CourseCategories { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TeacherSkills> TeacherSkills { get; set; }
    public DbSet<Degree> Degrees { get; set; }
    public DbSet<TeacherCategory> TeacherCategories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<HomeFooterSlider> HomeFooterSliders { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
}