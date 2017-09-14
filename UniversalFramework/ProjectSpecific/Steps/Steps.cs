using System;

namespace ProjectSpecific.Steps
{
    public class Steps
    {


        private Lazy<StepsTimeSeriesAnalysis> _timeSeriesAnalysis = new Lazy<StepsTimeSeriesAnalysis>();

        public StepsTimeSeriesAnalysis TimeSeriesAnalysis
        {
            get
            {
                return _timeSeriesAnalysis.Value;
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
