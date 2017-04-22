using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSpecific.BO
{
    public class SampleObject
    {
        string a = "param a";

        int b = 12;

        public override string ToString()
        {
            return "complex object with " + a + " = " + b.ToString();
        }
    }
}
