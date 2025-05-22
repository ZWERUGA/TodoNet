using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Dtos.Todo;
using Todo.Mappers;

namespace Todo.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController(TodoDbContext context) : ControllerBase
    {
        private readonly TodoDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _context.Todos.ToListAsync();
            var todosDto = todos.Select(t => t.ToTodoDtoFromTodoModel());

            return Ok(todosDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo.ToTodoDtoFromTodoModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoDto createTodoDto)
        {
            var todoModel = createTodoDto.ToTodoModelFromCreateTodoDto();

            await _context.Todos.AddAsync(todoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = todoModel.Id },
                todoModel.ToTodoDtoFromTodoModel()
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateTodoDto updateTodoDto
        )
        {
            var todoModel = await _context.Todos.FindAsync(id);

            if (todoModel is null)
            {
                return NotFound();
            }

            todoModel.Title = updateTodoDto.Title;
            todoModel.Text = updateTodoDto.Text;
            todoModel.IsCompleted = updateTodoDto.IsCompleted;
            todoModel.IsFavorite = updateTodoDto.IsFavorite;

            await _context.SaveChangesAsync();

            return Ok(todoModel.ToTodoDtoFromTodoModel());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var todoModel = await _context.Todos.FindAsync(id);

            if (todoModel is null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
