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

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateTodoDto updateTodoDto)
        {
            var todoModel = _context.Todos.Find(id);

            if (todoModel is null)
            {
                return NotFound();
            }

            todoModel.Title = updateTodoDto.Title;
            todoModel.Text = updateTodoDto.Text;
            todoModel.IsCompleted = updateTodoDto.IsCompleted;
            todoModel.IsFavorite = updateTodoDto.IsFavorite;

            _context.SaveChanges();

            return Ok(todoModel.ToTodoDtoFromTodoModel());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var todoModel = _context.Todos.Find(id);

            if (todoModel is null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todoModel);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
