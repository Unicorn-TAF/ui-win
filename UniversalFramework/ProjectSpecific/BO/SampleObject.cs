using System;

namespace ProjectSpecific.BO
{
    public class SampleObject
    {
        public SampleObject()
        {

        }

        public SampleObject(string a, int b)
        {
            this.a = a;
            this.b = b;
        }

        string a = "param a";

        int b = 12;

        public override string ToString()
        {
            return "complex object with " + a + " = " + b.ToString();
        }

        public override bool Equals(object obj)
        {
            SampleObject _obj = obj as SampleObject;
            return a == _obj.a && b == _obj.b;
        }
    }
}
