using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Dtos.Todo;
using Todo.Helpers;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Repository
{
    public class TodoRepository(TodoDbContext context) : ITodoRepository
    {
        private readonly TodoDbContext _context = context;

        public async Task<List<TodoModel>> GetAllAsync(QueryObject query, string appUserId)
        {
            var todos = _context.Todos.Where(u => u.AppUserId == appUserId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
                todos = todos.Where(t => EF.Functions.ILike(t.Title, $"%{query.Title}%"));

            if (!string.IsNullOrWhiteSpace(query.Text))
                todos = todos.Where(t => EF.Functions.ILike(t.Text, $"%{query.Text}%"));

            if (query.IsCompleted.HasValue)
                todos = todos.Where(t => t.IsCompleted == query.IsCompleted.Value);

            if (query.IsFavorite.HasValue)
                todos = todos.Where(t => t.IsFavorite == query.IsFavorite.Value);

            return await todos.ToListAsync();
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

            if (updateTodoDto.Title is not null)
                existingTodo.Title = updateTodoDto.Title;

            if (updateTodoDto.Text is not null)
                existingTodo.Text = updateTodoDto.Text;

            if (updateTodoDto.IsCompleted.HasValue)
                existingTodo.IsCompleted = updateTodoDto.IsCompleted.Value;

            if (updateTodoDto.IsFavorite.HasValue)
                existingTodo.IsFavorite = updateTodoDto.IsFavorite.Value;

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
