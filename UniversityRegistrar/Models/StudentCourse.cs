using System.Collections.Generic;

namespace UniversityRegistrar.Models
{
  public class StudentCourse
  {
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int StudentCourseId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public bool CourseCompleted { get; set; } = false;

    public void ChangeCourseStatus()
    {
      if(!CourseCompleted)
      {
        CourseCompleted = true;
      }
      else
      {
        CourseCompleted = false;
      }
    }
  }
}