using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace WebApplication2.Controllers
{


    [Route("Tasks")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;


        public TasksController(AppDbContext context)
        {
            _db = context;
        }



        [HttpGet("Index/{projectId:int}")]
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .ToList();


            ViewBag.ProjectId = projectId;
            return View(tasks);
        }




        [HttpGet("Details/{id:int}")]

        public IActionResult Details(int id)

        {
            var task = _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }
            return View(task);

        }


        [HttpGet("Create/{projectId:int}")]
        public IActionResult Create(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }


            var task = new ProjectTask
            {
                ProjectId = projectId
            };

            return View(task);
        }


        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _db.ProjectTasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }



        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var task = _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);


            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }



        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }


        [HttpGet("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var task = _db.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


        [HttpPost("DeleteConfirmed/{id:int}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirmed(int ProjectTaskId)
        {
            var task = _db.ProjectTasks.Find(ProjectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            return NotFound();
        }


       
		[HttpGet("Search/{projectId:int}/{searchString?}")]
		public async Task<IActionResult> Search(int projectId, string searchString)
		{
			// var tasksQuery = _db.ProjectTasks.Where(t => t.ProjectId == projectId);
			var tasksQuery = _db.ProjectTasks.AsQueryable();

			if (!String.IsNullOrEmpty(searchString))
			{
				tasksQuery = tasksQuery.Where(t => t.Title.Contains(searchString)
												   || t.Description.Contains(searchString));
			}

			var tasks = await tasksQuery.ToListAsync();
			ViewBag.ProjectId = projectId; // To keep track of the current project
			return View("Index", tasks); // Reuse the Index view to display results
		}


	}
}
