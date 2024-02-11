using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concreate
{
    public class Order : BaseEntity, IEntity
    {
        public string OrderNumber { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
