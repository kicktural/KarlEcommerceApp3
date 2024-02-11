using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concreate
{
    public class Product : BaseEntity, IEntity
    {
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public bool Stock { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDeleted { get; set; }
        public bool Status { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<Order> Orders { get; set; }
        public List<ProductLanguage> ProductLanguages { get; set; }
    }
}
