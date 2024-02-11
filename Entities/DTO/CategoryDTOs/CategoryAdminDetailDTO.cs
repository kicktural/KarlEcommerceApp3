using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.CategoryDTOs
{
    public class CategoryAdminDetailDTO
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<string> CategoryName { get; set; }
    }
}
