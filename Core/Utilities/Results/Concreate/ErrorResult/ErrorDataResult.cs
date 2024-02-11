using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concreate.ErrorResult
{
    public class ErrorDataResult<T> : DataResult<T>
    {

        public ErrorDataResult(T data, string message) : base(data, false, message) //error mesaj yaza biler ve false sekilde gondere biler
        {
        }
        public ErrorDataResult(T data) : base(data, false) // null gondermek istese (mesaj yazmadan)
        {
        }
        public ErrorDataResult(string message) : base(default, false, message) // null data gelse default (hazir) deyer gonderecek
        {
        }
        public ErrorDataResult() : base(default, false) // sadece error gondermek istesen
        {
        }
    }
}
