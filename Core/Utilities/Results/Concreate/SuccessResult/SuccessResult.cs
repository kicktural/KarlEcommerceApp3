using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concreate.SuccessResult
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message) // mesaj yaza biler ver true olaraq gondere biler (mesaj yazsa default True gonderilecek)
        {
        }

        public SuccessResult() : base(true) //mesaj yazmadan gondere biler (mesaj yazsa default True gonderilecek)
        {
        }
    }
}
