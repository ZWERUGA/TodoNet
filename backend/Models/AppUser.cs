using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Todo.Models
{
    public class AppUser : IdentityUser
    {
        public List<TodoModel> Todos { get; set; } = [];
    }
}
