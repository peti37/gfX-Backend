using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gfX.Models.ViewModels
{
    public class UserDTO
    {
        public string GithubHandle { get; set; }
        public string Name { get; set; }
        public bool HasJob { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public List<string> ReposToShow { get; set; }
    }
}
