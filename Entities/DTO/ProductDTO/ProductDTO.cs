using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ProductDTO
{
    public class ProductDTO
    {
        public record ProductEditRecordDTO(int Id, decimal Price, decimal DisCount, int Quantity, int CategoryId, bool IsFeatured, List<string> ProductNames, List<string> Descriptions, List<string> SeoUrls, List<string> PhotoUrls);
    }
}
