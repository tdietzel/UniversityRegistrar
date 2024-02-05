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
  }
}