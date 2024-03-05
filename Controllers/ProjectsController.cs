using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;


/*using Microsoft.CodeAnalysis;*/




namespace WebApplication2.Controllers
{

    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;

        public ProjectsController(AppDbContext db)
        {
            _db = db;

        }
        /*private static List<Project> _projects = new List<Project>()
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project" }
        };*/

        public IActionResult Index()
        {
            return View(_db.Projects.ToList());
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


		[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _db.Projects.Add(project);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }



        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExsists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return View(project);
        }


        private bool ProjectExsists(int id)
        {
            return _db.Projects.Any(e => e.ProjectId == id);
        }


        public IActionResult Delete(int id)
        {
            var project = _db.Projects.FirstOrDefault(p =>p.ProjectId == id);
            if (project==null)
            {
                return NotFound();
            }
            return View(project);
        }



        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirmed(int ProjectId)
        {
			var project = _db.Projects.Find(ProjectId);
			if (project != null)
            {
                _db.Projects.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();

		}




        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = from p in _db.Projects
                                select p;

            bool searchPerformed = !String.IsNullOrEmpty(searchString);

            if (searchPerformed)
            {
                projectsQuery = projectsQuery.Where(p => p.Name.Contains(searchString)
                                || p.Description.Contains(searchString));
            }

            var projects = await projectsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", projects); 








    }
}
