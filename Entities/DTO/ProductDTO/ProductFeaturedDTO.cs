﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ProductDTO
{
    public class ProductFeaturedDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string PhotoUrl { get; set; }
        public string  SeoUrl { get; set; }
    }
}
