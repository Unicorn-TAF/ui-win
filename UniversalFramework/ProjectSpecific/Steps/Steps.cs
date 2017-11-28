using System;

namespace ProjectSpecific.Steps
{
    public class Steps
    {


        private Lazy<StepsCharMap> _charmap = new Lazy<StepsCharMap>();

        public StepsCharMap CharMap
        {
            get
            {
                return _charmap.Value;
            }
        }


        private Lazy<StepsYandexMarket> _yandexMarket = new Lazy<StepsYandexMarket>();

        public StepsYandexMarket YandexMarket
        {
            get
            {
                return _yandexMarket.Value;
            }
        }


        private Lazy<TestingSteps> _testing = new Lazy<TestingSteps>();

        public TestingSteps Testing
        {
            get
            {
                return _testing.Value;
            }
        }

    }
}
