using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concreate.ErrorResult
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message)  // mesaj yaza biler ver false olaraq gondere biler
        {
        }

        public ErrorResult() : base(false)  //mesaj yazmadan gondere biler
        {
        }
    }
}
