using System.ComponentModel.DataAnnotations;

namespace Todo.Dtos.Todo
{
    public class CreateTodoDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Заголовок должен состоять минимум из 3 символов.")]
        [MaxLength(30, ErrorMessage = "Заголовок должен состоять максимум из 30 символов.")]
        public required string Title { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Текст должен состоять минимум из 3 символов.")]
        [MaxLength(200, ErrorMessage = "Текст должен состоять максимум из 200 символов.")]
        public required string Text { get; set; }
    }
}
