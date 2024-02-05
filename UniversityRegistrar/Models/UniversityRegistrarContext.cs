using Microsoft.EntityFrameworkCore;

namespace UniversityRegistrar.Models
{
  public class UniversityRegistrarContext : DbContext
  {
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<StudentDepartment> StudentDepartments { get; set; }
    public DbSet<CourseDepartment> CourseDepartments { get; set; }
    public UniversityRegistrarContext(DbContextOptions options) : base(options) { }
  }
}
