using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Dtos.Todo;
using Todo.Extensions;
using Todo.Helpers;
using Todo.Interfaces;
using Todo.Mappers;
using Todo.Models;

namespace Todo.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController(ITodoRepository todoRepo, UserManager<AppUser> userManager)
        : ControllerBase
    {
        private readonly ITodoRepository _todoRepo = todoRepo;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var userName = User.GetUsername();
            if (userName is null)
                return Unauthorized("Пользователь не найден.");

            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser is null)
                return Unauthorized("Пользователь не найден.");

            var todos = await _todoRepo.GetAllAsync(query, appUser.Id);
            var todosDto = todos.Select(t => t.ToTodoDtoFromTodoModel());

            return Ok(todosDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTodoDto createTodoDto)
        {
            var username = User.GetUsername();
            if (username is null)
                return Unauthorized("Для создания задачи необходимо войти в систему.");

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser is null)
                return Unauthorized("Пользователь не найден.");

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
            var todoModel = await _todoRepo.UpdateAsync(id, updateTodoDto);

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
            var todoModel = await _todoRepo.DeleteAsync(id);

            if (todoModel is null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
