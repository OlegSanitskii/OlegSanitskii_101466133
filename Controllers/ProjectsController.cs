using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;




namespace WebApplication2.Controllers
{
    public class ProjectsController : Controller
    {
        /*private static List<Project> _projects = new List<Project>()
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project" }
        };*/

        public IActionResult Index()
        {
            return View(_db.projects.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            if (project != null)
            {
                _projects.Add(project);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
    }
}
