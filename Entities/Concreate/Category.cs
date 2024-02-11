using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concreate
{
    public class Category : BaseEntity, IEntity
    {  
        public string PhotoUrl { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDelated { get; set; }
        public List<Product> Products { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
    }
}
