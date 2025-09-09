using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

[Route("api/tasks")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // Obtener todas las tasks del usuario
    [HttpGet]
    public IActionResult GetTasks()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var tasks = _context.Tasks.Where(t => t.UserId == userId).ToList();
        return Ok(tasks);
    }

    // Crear una nueva task
    [HttpPost]
    public IActionResult CreateTask([FromBody] Task task)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        // Validar categoría
        var category = _context.Categories.FirstOrDefault(c => c.Id == task.CategoryId);
        if (category == null)
        {
            return BadRequest(new { message = "Invalid category. Must be 'To Do', 'In Progress' or 'Done'." });
        }

        var newTask = new Task
        {
            Title = task.Title,
            Description = task.Description,
            UserId = userId.Value,
            CategoryId = task.CategoryId
        };

        _context.Tasks.Add(newTask);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTasks), new { id = newTask.Id }, newTask);
    }

    // Actualizar una task
    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] Task updatedTask)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        // Validar categoría
        var category = _context.Categories.FirstOrDefault(c => c.Id == updatedTask.CategoryId);
        if (category == null)
        {
            return BadRequest(new { message = "Invalid category. Must be 'To Do', 'In Progress' or 'Done'." });
        }

        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.CategoryId = updatedTask.CategoryId;
        _context.SaveChanges();

        return NoContent();
    }

    // Eliminar una task
    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        _context.SaveChanges();

        return NoContent();
    }

    // Asignar categoría a una task
    [HttpPost("{id}/category/{categoryId}")]
    public IActionResult AddCategoryToTask(int id, int categoryId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
        var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        if (task == null || category == null) return NotFound();

        task.CategoryId = categoryId;
        _context.SaveChanges();

        return Ok(task);
    }

    // Obtener tasks por categoría
    [HttpGet("category/{categoryId}")]
    public IActionResult GetTasksByCategory(int categoryId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var tasks = _context.Tasks.Where(t => t.UserId == userId && t.CategoryId == categoryId).ToList();
        return Ok(tasks);
    }

    // Obtener categorías (fijas)
    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
        var categories = _context.Categories.ToList();
        return Ok(categories);
    }
}
