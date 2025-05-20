using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Dtos.Todo
{
    public class CreateTodoDto
    {
        public required string Title { get; set; }
        public required string Text { get; set; }
    }
}
