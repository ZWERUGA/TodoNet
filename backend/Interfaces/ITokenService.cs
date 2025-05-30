using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
