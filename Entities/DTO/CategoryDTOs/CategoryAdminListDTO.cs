﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.CategoryDTOs
{
    public class CategoryAdminListDTO
    {
        public int Id  { get; set; }
        public string CategoryName { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsFeatured { get; set; }    
    }
}
