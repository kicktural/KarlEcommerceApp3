using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concreate
{
    public class Picture : BaseEntity, IEntity
    {
        public string PhotoUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }  //bir mehsulun bir nece dene sekli ola biler.
    }
}
