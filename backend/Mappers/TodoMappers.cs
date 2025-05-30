using Todo.Dtos.Todo;
using Todo.Models;

namespace Todo.Mappers
{
    public static class TodoMappers
    {
        public static TodoDto ToTodoDtoFromTodoModel(this TodoModel todoModel)
        {
            return new TodoDto
            {
                Id = todoModel.Id,
                Title = todoModel.Title,
                Text = todoModel.Text,
                IsCompleted = todoModel.IsCompleted,
                IsFavorite = todoModel.IsFavorite,
            };
        }

        public static TodoModel ToTodoModelFromCreateTodoDto(
            this CreateTodoDto createTodoDto,
            string appUserId
        )
        {
            return new TodoModel
            {
                Title = createTodoDto.Title,
                Text = createTodoDto.Text,
                IsCompleted = false,
                IsFavorite = false,
                AppUserId = appUserId,
            };
        }
    }
}
