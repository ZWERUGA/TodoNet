using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Dtos.Todo
{
    public class TodoDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Text { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsFavorite { get; set; } = false;
    }
}
