using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
  public class CourseDepartment
  {
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int CourseDepartmentId { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }

  }
}