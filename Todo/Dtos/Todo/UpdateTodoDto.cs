namespace Todo.Dtos.Todo
{
    public class UpdateTodoDto
    {
        public required string Title { get; set; }
        public required string Text { get; set; }
        public required bool IsCompleted { get; set; }
        public required bool IsFavorite { get; set; }
    }
}
