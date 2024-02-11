﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ProductDTO
{
    public class ProductAdminListDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        //public string Email { get; set; }
    }
}
