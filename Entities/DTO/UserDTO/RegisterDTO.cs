using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.UserDTO
{
    public class RegisterDTO
    {
        /// <summary>
        /// UserName
        /// </summary>
     
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Error LastName")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
