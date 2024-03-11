using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concreate.EntityFremawork
{
    public class AppUser : IdentityUser, IEntity
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string?  Picture { get; set; }
    }
}
