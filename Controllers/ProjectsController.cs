using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;




namespace WebApplication2.Controllers
{
    public class ProjectsController : Controller
    {



        public IActionResult Index()
        {
            var projects = new List<Project>()
                {
                    new Project { ProjectId = 1, Name = "Project 1", Description = "First Project" },
                    new Project { ProjectId = 2, Name = "Project 2", Description = "Second Project" }
                };
        
                return View(projects);
            }



        [HttpGet]
        public IActionResult Details(int id) 
        
        {
            var project = new Project { ProjectId = id, Name = "Project " + id, Description = "Deitails of Project " + id };

            return View(project);
           
        }




        
        public IActionResult Create()
        
        {
            return View(); 
        }

    }
}
