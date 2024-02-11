using Entities.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.CategoryDTOs
{
    public class CategoryDetailDTO
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string CategoryName { get; set; }
        public bool IsFeatured { get; set; }
    }
}
