using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Helpers
{
    public class QueryObject
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsFavorite { get; set; }
    }
}
