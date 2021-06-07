namespace Demo.Specifics.BO
{
    public class DemoObject
    {
        public DemoObject() : this("some string", 12)
        {
        }

        public DemoObject(string parameter1, int parameter2)
        {
            Property1 = parameter1;
            Property2 = parameter2;
        }

        public string Property1 { get; protected set; }

        public int Property2 { get; protected set; }

        public override string ToString() =>
            $"Demo object with {nameof(Property1)} = {Property1} and {nameof(Property2)} = {Property2}";

        public override bool Equals(object obj)
        {
            DemoObject sampleObj = obj as DemoObject;
            return Property1 == sampleObj.Property1 && Property2 == sampleObj.Property2;
        }

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
