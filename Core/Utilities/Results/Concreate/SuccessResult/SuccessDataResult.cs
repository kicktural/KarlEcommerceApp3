using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concreate.SuccessResult
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        //ErrorDataResult da olan error, null, data, ucun constractor'lar SuccessDataResult (true) ucun kecerlidir. ferq ondadirki false yerine true olacaq. 
        public SuccessDataResult(T data,  string message) : base(data,  true, message) //true (success) mesaj yaza biler ve true sekilde gondere biler
        {
        }
        public SuccessDataResult(T data) : base(data, true) // null gondermek istese (mesaj yazmadan)
        {
        }

        public SuccessDataResult(string message) : base(default, true, message) // null data gelse default (hazir) deyer gonderecek
        {
        }
        public SuccessDataResult() : base(default, true) // sadece true (success) gondermek istesen
        {
        }
    }
}
