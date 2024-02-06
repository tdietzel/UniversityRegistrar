using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace UniversityRegistrar.Controllers
{
  public class StudentsController : Controller
  {
    private readonly UniversityRegistrarContext _db;

    public StudentsController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Students.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Student student)
    {
      _db.Students.Add(student);
      _db.SaveChanges();

      List<Student> model = _db.Students.ToList();
      return View("Index", model);
    }


    public SelectList DepartmentSelectList(int? id = null)
    {
      SelectList departmentList = new(_db.Departments, "DepartmentId", "DepartmentName");
      if (id != null)
      {
        foreach (SelectListItem dept in departmentList)
        {
          if (dept.Value == id.ToString())
          {
            dept.Selected = true;
          }
        }
      }
      return departmentList;
    }

    public SelectList StudentCourseCompletionStatusSelectList(bool? status = false)
    {
      SelectList courseStatus = new(_db.StudentCourses, "CourseCompleted", "CourseCompleted");
      if (status != false)
      {
        foreach (SelectListItem course in courseStatus)
        {
          if (course.Value == status.ToString())
          {
            course.Selected = true;
          }
        }
      }
      return courseStatus;
    }

    public ActionResult Details(int id)
    {
      Student selectedStudent = _db.Students
                                    .Include(s => s.JoinEntities)
                                    .ThenInclude(join => join.Course)
                                    .Include(s => s.MoreJoinEntities)
                                    .ThenInclude(join => join.Department)
                                    .FirstOrDefault(s => s.StudentId == id);
      return View(selectedStudent);
    }

    public ActionResult Edit(int id)
    {
      Student selectedStudent = _db.Students
                                    .Include(s => s.JoinEntities)
                                    .ThenInclude(join => join.Course)
                                    .Include(s => s.MoreJoinEntities)
                                    .ThenInclude(join => join.Department)
                                    .FirstOrDefault(s => s.StudentId == id);
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseName");
      // ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
      ViewBag.DepartmentId = DepartmentSelectList(selectedStudent.MoreJoinEntities[0].Department.DepartmentId);
      // ViewBag.CourseCompleted = StudentCourseCompletionStatusSelectList(selectedStudent.JoinEntities[id].CourseCompleted);
      return View(selectedStudent);
    }

    [HttpPost]
    public ActionResult Edit (Student student, int departmentId)
    {
      _db.Students.Update(student);
      _db.SaveChanges();

      
      Department selectedDepartment = _db.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);

      #nullable enable
      StudentDepartment? joinEntity = _db.StudentDepartments.FirstOrDefault(join => (join.StudentId == student.StudentId));
      #nullable disable
      if (joinEntity == null && student.StudentId != 0)
      {
        _db.StudentDepartments.Add(new StudentDepartment() {StudentId = student.StudentId, DepartmentId = departmentId});
        _db.SaveChanges();
      } 
      else
      {
        _db.StudentDepartments.Remove(joinEntity);
        _db.SaveChanges();
        _db.StudentDepartments.Add(new StudentDepartment() {StudentId = student.StudentId, DepartmentId = departmentId});
        _db.SaveChanges();
      }

      return RedirectToAction("Details", new {id = student.StudentId});
    }

    [HttpPost, ActionName("Details")]
    public ActionResult ChangeCompletionStatus (int studentCourseId)
    {
      StudentCourse studentCourse = _db.StudentCourses.FirstOrDefault(entry => entry.StudentCourseId == studentCourseId);
      studentCourse.ChangeCourseStatus();
      _db.StudentCourses.Update(studentCourse);
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = studentCourse.StudentId});
    }
  }
}