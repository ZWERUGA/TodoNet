using System.ComponentModel.DataAnnotations;

namespace Todo.Dtos.Todo
{
    public class UpdateTodoDto
    {
        [MinLength(3, ErrorMessage = "Заголовок должен состоять минимум из 3 символов.")]
        [MaxLength(30, ErrorMessage = "Заголовок должен состоять максимум из 30 символов.")]
        public string? Title { get; set; }

        [MinLength(3, ErrorMessage = "Текст должен состоять минимум из 3 символов.")]
        [MaxLength(200, ErrorMessage = "Текст должен состоять максимум из 200 символов.")]
        public string? Text { get; set; }

        public bool? IsCompleted { get; set; }
        public bool? IsFavorite { get; set; }
    }
}
