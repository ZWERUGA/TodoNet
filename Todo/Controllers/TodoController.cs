using Microsoft.AspNetCore.Mvc;
using Todo.Dtos.Todo;
using Todo.Interfaces;
using Todo.Mappers;

namespace Todo.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController(ITodoRepository todoRepo) : ControllerBase
    {
        private readonly ITodoRepository _todoRepo = todoRepo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _todoRepo.GetAllAsync();
            var todosDto = todos.Select(t => t.ToTodoDtoFromTodoModel());

            return Ok(todosDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var todo = await _todoRepo.GetByIdAsync(id);

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

            await _todoRepo.CreateAsync(todoModel);

            return CreatedAtAction(
                nameof(GetById),
                new { id = todoModel.Id },
                todoModel.ToTodoDtoFromTodoModel()
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateTodoDto updateTodoDto
        )
        {
            var todoModel = await _todoRepo.UpdateAsync(id, updateTodoDto);

            if (todoModel is null)
            {
                return NotFound();
            }

            return Ok(todoModel.ToTodoDtoFromTodoModel());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var todoModel = await _todoRepo.DeleteAsync(id);

            if (todoModel is null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
