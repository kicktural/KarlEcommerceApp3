using Core.DataAccess.EntityFremawork;
using DataAccess.Abstract;
using Entities.Concreate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concreate.SQLServer
{
    public class EFPictureDAL : EFRepositoryBase<Picture, AppDbContext>, IPictureDAL
    {
        public async Task<bool> RemovePictureAsync(string url)
        {
            try
            {
            using var context = new AppDbContext();
            var result = await context.Pictures.FirstOrDefaultAsync(x => x.PhotoUrl == url);
            context.Pictures.Remove(result);
             context.SaveChanges();
            return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
