using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concreate
{
    public class PictureManager : IPictureService
    {
        private readonly IPictureDAL _pictureDAL;

        public PictureManager(IPictureDAL pictureDAL)
        {
            _pictureDAL = pictureDAL;
        }

        public async Task<IResult> RemoveProductPictureAsync(string url)
        {

            try
            {
            var result = await _pictureDAL.RemovePictureAsync(url);
            if (result)
                return new SuccessResult("Success Remove!");

            return new ErrorResult();
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
        }

        //Task<IResult> IPictureService.RemoveProductPictureAsync(string url)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
