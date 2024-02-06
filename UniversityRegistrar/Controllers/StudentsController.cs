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
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
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
        Console.WriteLine("Entity null");
        _db.StudentDepartments.Add(new StudentDepartment() {StudentId = student.StudentId, DepartmentId = departmentId});
        _db.SaveChanges();
      } 
      else
      {
        Console.WriteLine("Entity good");
        _db.StudentDepartments.Remove(joinEntity);
        _db.StudentDepartments.Add(new StudentDepartment() {StudentId = student.StudentId, DepartmentId = departmentId});
      }

      return RedirectToAction("Details", new {id = student.StudentId});
    }
  }
}