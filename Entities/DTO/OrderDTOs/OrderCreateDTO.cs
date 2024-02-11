using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.OrderDTOs
{
    public class OrderCreateDTO
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string Message { get; set; }
    }
}
