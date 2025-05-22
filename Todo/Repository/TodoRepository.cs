using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Dtos.Todo;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Repository
{
    public class TodoRepository(TodoDbContext context) : ITodoRepository
    {
        private readonly TodoDbContext _context = context;

        public async Task<List<TodoModel>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<TodoModel?> GetByIdAsync(int todoId)
        {
            return await _context.Todos.FindAsync(todoId);
        }

        public async Task<TodoModel> CreateAsync(TodoModel todoModel)
        {
            await _context.Todos.AddAsync(todoModel);
            await _context.SaveChangesAsync();

            return todoModel;
        }

        public async Task<TodoModel?> UpdateAsync(int todoId, UpdateTodoDto updateTodoDto)
        {
            var existingTodo = await _context.Todos.FindAsync(todoId);

            if (existingTodo is null)
                return null;

            existingTodo.Title = updateTodoDto.Title;
            existingTodo.Text = updateTodoDto.Text;
            existingTodo.IsCompleted = updateTodoDto.IsCompleted;
            existingTodo.IsFavorite = updateTodoDto.IsFavorite;

            await _context.SaveChangesAsync();

            return existingTodo;
        }

        public async Task<TodoModel?> DeleteAsync(int todoId)
        {
            var existingTodo = await _context.Todos.FindAsync(todoId);

            if (existingTodo is null)
                return null;

            _context.Todos.Remove(existingTodo);
            await _context.SaveChangesAsync();

            return existingTodo;
        }
    }
}
