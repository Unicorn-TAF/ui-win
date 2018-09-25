namespace Unicorn.UnitTests.BO
{
    public class SampleObject
    {
        private readonly string a = "param a";
        private readonly int b = 12;

        public SampleObject()
        {
        }

        public SampleObject(string a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public override string ToString()
        {
            return "complex object with " + a + " = " + b.ToString();
        }

        public override bool Equals(object obj)
        {
            SampleObject sampleObj = obj as SampleObject;
            return a == sampleObj.a && b == sampleObj.b;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
