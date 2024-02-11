using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using DataAccess.Abstract;
using DataAccess.Concreate.SQLServer;
using Entities.Concreate;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concreate
{
    public class UserManager : IUserService
    {
        private readonly IUserDAL _userDAL;

        public UserManager(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public IDataResult<User> GetUserById(string userId)
        {
            using AppDbContext context = new();

            var result = _userDAL.Get(x => x.Id == userId);
            //if (result == null)
            //{
            //    return null;
            //}
            return new SuccessDataResult<User>(result);
        }
    }
}
