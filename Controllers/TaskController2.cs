using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;


namespace WebApplication2.Controllers
{
    public class TaskController2 : Controller
    {
        private readonly AppDbContext _db;

        public TaskController2(AppDbContext db)
        {
            _db = db;

        }




        public IActionResult Index(int projectId)
        {

            var taskName = "Mytask";

            var result = _db.ProjectTasks.Where(current => current.Title == taskName)
                .ToList();

            result = _db.ProjectTasks.Where(current => current.Title.StartsWith(taskName))
                .ToList();

            result = _db.ProjectTasks.Where(current => current.Title.Contains(taskName))
                .ToList();

            result = _db.ProjectTasks.Where(current => current.Title.Contains(taskName) || current.Description.Contains(taskName))
                .ToList();








            var tasks = _db.ProjectTasks
                .Where(tasks =>tasks.ProjectId == projectId)
                .ToList();
            ViewBag.ProjectId = projectId;




            return View(tasks);
        }
    }
}
