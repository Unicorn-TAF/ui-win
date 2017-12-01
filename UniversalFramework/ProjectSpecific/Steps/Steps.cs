using System;

namespace ProjectSpecific.Steps
{
    public class Steps
    {
        private Lazy<StepsCharMap> _charmap = new Lazy<StepsCharMap>();
        public StepsCharMap CharMap => _charmap.Value;

        private Lazy<StepsYandexMarket> _yandexMarket = new Lazy<StepsYandexMarket>();
        public StepsYandexMarket YandexMarket => _yandexMarket.Value;

        private Lazy<TestingSteps> _testing = new Lazy<TestingSteps>();
        public TestingSteps Testing => _testing.Value;

        private Lazy<StepsAndroid> _android = new Lazy<StepsAndroid>();

        public StepsAndroid Android => _android.Value;
    }
}
