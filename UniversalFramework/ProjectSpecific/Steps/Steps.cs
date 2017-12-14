using System;

namespace ProjectSpecific.Steps
{
    public class Steps
    {
        private Lazy<StepsCharMap> charmap = new Lazy<StepsCharMap>();
        private Lazy<StepsYandexMarket> yandexMarket = new Lazy<StepsYandexMarket>();
        private Lazy<TestingSteps> testing = new Lazy<TestingSteps>();
        private Lazy<StepsAndroid> android = new Lazy<StepsAndroid>();
        private Lazy<StepsiOS> iOs = new Lazy<StepsiOS>();

        public StepsCharMap CharMap => charmap.Value;

        public StepsYandexMarket YandexMarket => yandexMarket.Value;

        public TestingSteps Testing => testing.Value;

        public StepsAndroid Android => android.Value;

        public StepsiOS IOS => iOs.Value;
    }
}
