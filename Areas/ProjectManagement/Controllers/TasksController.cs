using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication2.Areas.ProjectManagement.Models;



namespace WebApplication2.Areas.ProjectManagement.Controllers
{


    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;


        public TasksController(AppDbContext context)
        {
            _db = context;
        }



        [HttpGet("Index/{projectId:int}")]
        public async Task<ActionResult> Index(int? projectId)
        {
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId.Value);
            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId;  // Store projectId in ViewBag
            return View(tasks);
        }




        // GET: Tasks/Details/{id}
        [HttpGet("Details/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project) // Include related project data
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


        // GET: Tasks/Create/{projectId}
        [HttpGet("Create/{projectId:int}")]
        public async Task<ActionResult> Create(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound(); // Or handle appropriately if project doesn't exist
            }

            var task = new ProjectTask
            {
                ProjectId = projectId
            };

            return View(task);
        }


        // POST: Tasks/Create
        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _db.ProjectTasks.AddAsync(task);
                await _db.SaveChangesAsync();
                // Redirect to the Index action with the projectId of the created task
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            // Async call to retrieve projects for SelectList
            var projects = await _db.Projects.ToListAsync();

            // Repopulate the Projects SelectList if returning to the form
            ViewBag.Projects = new SelectList(projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }



        // GET: Tasks/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project) // Include related project data
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            // Async call to retrieve projects for SelectList
            var projects = await _db.Projects.ToListAsync();

            // Repopulate the Projects SelectList if returning to the form
            ViewBag.Projects = new SelectList(projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }



        // POST: Tasks/Edit/{id}
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            // Async call to retrieve projects for SelectList
            var projects = await _db.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }


        // GET: Tasks/Delete/{id}
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project) // Include related project data
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }



        // POST: Tasks/DeleteConfirmed/{id}
        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectTaskId)
        {
            var task = await _db.ProjectTasks.FindAsync(ProjectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            return NotFound();
        }



        [HttpGet("Search/{projectId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int projectId, string searchString)
        {
            // var tasksQuery = _db.ProjectTasks.Where(t => t.ProjectId == projectId);
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
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
