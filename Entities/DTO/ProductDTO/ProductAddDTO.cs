using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ProductDTO
{
    public class ProductAddDTO
    {
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public int CategoryId { get; set; }
        public bool IsFeatured { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public List<string> ProductNames { get; set; }
        public  List<string> Descriptions { get; set; }
        public List<string> LangCodes { get; set; }
        public List<string> SeoUrls { get; set; }
        public List<string> PhotoUrls { get; set; }
    }
}
