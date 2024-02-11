using Core.Entities;
using Core.Entities.Concreate.EntityFremawork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concreate
{
    public class User : AppUser
    {
        public string Address { get; set; }
    }
}
