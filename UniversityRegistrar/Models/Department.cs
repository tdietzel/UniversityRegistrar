using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
  public class Department
  {
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }

    public List<StudentDepartment> JoinEntities { get; }
  }
}