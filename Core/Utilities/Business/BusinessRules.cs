using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Check(params IResult[] logics)
        {
            foreach (var item in logics)
            {
                if (!item.Success)
                    return new ErrorResult("Error");
            }
            return new SuccessResult("Success");
        }
    }
}
