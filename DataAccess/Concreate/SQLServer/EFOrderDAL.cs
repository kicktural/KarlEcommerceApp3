using Core.DataAccess.EntityFremawork;
using DataAccess.Abstract;
using Entities.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concreate.SQLServer
{
    public class EFOrderDAL: EFRepositoryBase<Order, AppDbContext>, IOrderDAL
    {
        public void OrderAddRange(List<Order> orders)
        {
            using var context = new AppDbContext();

            context.Order.AddRange(orders);
            context.SaveChanges();
        }

    }
}
