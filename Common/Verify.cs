using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Verify
    {
        public bool StringLength(string str,int MinLength,int MaxLength)
        {
            if (String.IsNullOrWhiteSpace(str)) return false;
            if (str.Length > MaxLength) return false;
            if (str.Length < MinLength) return false;
            return true;
        }
    }
}
