using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                                    .FirstOrDefault(s => s.StudentId == id);
      return View(selectedStudent);
    }
  }
}