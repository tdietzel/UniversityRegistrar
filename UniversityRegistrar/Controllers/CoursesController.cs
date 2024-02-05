using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;


namespace UniversityRegistrar.Controllers
{
  public class CoursesController: Controller
  {
    private readonly UniversityRegistrarContext _db;

    public CoursesController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Courses.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Course course, int departmentId)
    {
      _db.Courses.Add(course);
      _db.SaveChanges();
      
      if (departmentId != null)
      {
        Department selectedDepartment = _db.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);

        #nullable enable
        CourseDepartment? joinEntity = _db.CourseDepartments.FirstOrDefault(join => (join.CourseId == course.CourseId && join.DepartmentId == selectedDepartment.DepartmentId));
        #nullable disable
        if (joinEntity == null && course.CourseId != 0)
        {
          _db.CourseDepartments.Add(new CourseDepartment() {CourseId = course.CourseId, DepartmentId = departmentId});
          _db.SaveChanges();
        }
      }
    
      return RedirectToAction("Index");
    }

    public ActionResult AddStudent(int id)
    {
      Course selectedCourse = _db.Courses.FirstOrDefault(c => c.CourseId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "StudentName");
      return View(selectedCourse);
    }

    [HttpPost]
    public ActionResult AddStudent(Course course, int studentId)
    {
      #nullable enable
      StudentCourse? joinEntity = _db.StudentCourses.FirstOrDefault(join => (join.StudentId == studentId && join.CourseId == course.CourseId));
      #nullable disable

      if(joinEntity == null & studentId != 0)
      {
        _db.StudentCourses.Add(new StudentCourse() { StudentId = studentId, CourseId = course.CourseId});
        _db.SaveChanges();
      }

      return RedirectToAction("Details", new { id = course.CourseId});
    }

    public ActionResult Details(int id)
    {
      Course selectedCourse = _db.Courses
                                  .Include(c => c.JoinEntities)
                                  .ThenInclude(join => join.Student)
                                  .FirstOrDefault(c => c.CourseId == id);
      return View(selectedCourse);
    }
  }
}