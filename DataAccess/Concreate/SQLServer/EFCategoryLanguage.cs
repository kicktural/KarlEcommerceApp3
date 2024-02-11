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
    public class EFCategoryLanguage : EFRepositoryBase<CategoryLanguage, AppDbContext>, ICategoryLanguage
    {
    }
}
