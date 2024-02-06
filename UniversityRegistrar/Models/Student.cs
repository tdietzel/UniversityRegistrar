using System;
using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public List<StudentCourse> JoinEntities { get; }
    public List<StudentDepartment> MoreJoinEntities { get; }
  }
}