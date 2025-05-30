namespace Todo.Models
{
    public class TodoModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Text { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsFavorite { get; set; } = false;
        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = null!;
    }
}
