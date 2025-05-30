using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Dtos.Todo;
using Todo.Helpers;
using Todo.Interfaces;
using Todo.Mappers;
using Todo.Services;

namespace Todo.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController(ITodoRepository todoRepo, UserService userService) : ControllerBase
    {
        private readonly ITodoRepository _todoRepo = todoRepo;
        private readonly UserService _userService = userService;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll([FromQuery] QueryObject query)
        {
            var (appUser, error) = await _userService.GetCurrentUserAsync(User);
            if (error is not null)
                return Unauthorized(error);

            var todos = await _todoRepo.GetAllAsync(query, appUser!.Id);
            var todosDto = todos.Select(t => t.ToTodoDtoFromTodoModel());

            return Ok(todosDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<TodoDto>> GetById([FromRoute] int id)
        {
            var (appUser, error) = await _userService.GetCurrentUserAsync(User);
            if (error is not null || appUser is null)
                return Unauthorized(error?.ToString() ?? "Пользователь не найден");

            var todo = await _todoRepo.GetByIdAsync(id, appUser.Id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo.ToTodoDtoFromTodoModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTodoDto createTodoDto)
        {
            var (appUser, error) = await _userService.GetCurrentUserAsync(User);
            if (error is not null || appUser is null)
                return Unauthorized(error?.ToString() ?? "Пользователь не найден");

            var todoModel = createTodoDto.ToTodoModelFromCreateTodoDto(appUser.Id);

            await _todoRepo.CreateAsync(todoModel);

            return CreatedAtAction(
                nameof(GetById),
                new { id = todoModel.Id },
                todoModel.ToTodoDtoFromTodoModel()
            );
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateTodoDto updateTodoDto
        )
        {
            var (appUser, error) = await _userService.GetCurrentUserAsync(User);
            if (error is not null || appUser is null)
                return Unauthorized(error?.ToString() ?? "Пользователь не найден");

            var todoModel = await _todoRepo.UpdateAsync(id, updateTodoDto, appUser.Id);

            if (todoModel is null)
            {
                return NotFound();
            }

            return Ok(todoModel.ToTodoDtoFromTodoModel());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var (appUser, error) = await _userService.GetCurrentUserAsync(User);
            if (error is not null || appUser is null)
                return Unauthorized(error?.ToString() ?? "Пользователь не найден");

            var todoModel = await _todoRepo.DeleteAsync(id, appUser.Id);

            if (todoModel is null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
