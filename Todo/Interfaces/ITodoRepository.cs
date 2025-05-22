using Todo.Dtos.Todo;
using Todo.Models;

namespace Todo.Interfaces
{
    public interface ITodoRepository
    {
        Task<List<TodoModel>> GetAllAsync();
        Task<TodoModel?> GetByIdAsync(int todoId);
        Task<TodoModel> CreateAsync(TodoModel todoModel);
        Task<TodoModel?> UpdateAsync(int todoId, UpdateTodoDto updateTodoDto);
        Task<TodoModel?> DeleteAsync(int todoId);
    }
}
