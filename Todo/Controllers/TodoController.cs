using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Dtos.Todo;
using Todo.Mappers;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(TodoDbContext context) : ControllerBase
    {
        private readonly TodoDbContext _context = context;

        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = _context.Todos.ToList();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var todo = _context.Todos.Find(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTodoDto createTodoDto)
        {
            var todoModel = createTodoDto.ToTodoModelFromCreateTodoDto();

            _context.Todos.Add(todoModel);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = todoModel.Id },
                todoModel.ToTodoDtoFromTodoModel()
            );
        }
    }
}
