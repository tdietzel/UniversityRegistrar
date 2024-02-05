using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UniversityRegistrar.Controllers
{
  public class DepartmentsController: Controller
  {
    private readonly UniversityRegistrarContext _db;
    public DepartmentsController(UniversityRegistrarContext db)
    {
      _db = db;
    }
    
    public ActionResult Index()
    {
      return View(_db.Departments.ToList());
    }

    public ActionResult Create() { return View(); }
    [HttpPost]
    public ActionResult Create(Department department)
    {
      _db.Departments.Add(department);
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    public ActionResult AddStudent(int id)
    {
      Department selectedDepartment = _db.Departments.FirstOrDefault(d => d.DepartmentId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "StudentName");
      return View(selectedDepartment);
    }

    [HttpPost]
    public ActionResult AddStudent(Department department, int studentId)
    {
      #nullable enable
      StudentDepartment? joinEntity = _db.StudentDepartments.FirstOrDefault(join => (join.StudentId == studentId && join.DepartmentId == department.DepartmentId));
      #nullable disable
      if (joinEntity == null && studentId != 0)
      {
        _db.StudentDepartments.Add(new StudentDepartment() {StudentId = studentId, DepartmentId = department.DepartmentId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = department.DepartmentId});
    }

    public ActionResult AddCourse(int id)
    {
      Department selectedDepartment = _db.Departments.FirstOrDefault(d => d.DepartmentId == id);
      ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseName");
      return View(selectedDepartment);
    }
    [HttpPost]
    public ActionResult AddCourse(Department department, int courseId)
    {
      #nullable enable
      CourseDepartment? joinEntity = _db.CourseDepartments.FirstOrDefault(join => (join.CourseId == courseId && join.DepartmentId == department.DepartmentId));
      #nullable disable
      if (joinEntity == null && courseId != 0)
      {
        _db.CourseDepartments.Add(new CourseDepartment() {CourseId = courseId, DepartmentId = department.DepartmentId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = department.DepartmentId });
    }

    public ActionResult Details(int id)
    {
      Department thisDepartment = _db.Departments.Include(department => department.JoinEntities)
      .ThenInclude(join => join.Student)
      .FirstOrDefault(department => department.DepartmentId == id);
      return View(thisDepartment);
    }
  }
}