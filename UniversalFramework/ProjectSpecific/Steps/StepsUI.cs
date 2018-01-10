using System;
using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.Core.Testing.Verification;
using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsUI : TestSteps
    {
        private Lazy<StepsCharMap> charmap = new Lazy<StepsCharMap>();
        private Lazy<StepsYandexMarket> yandexMarket = new Lazy<StepsYandexMarket>();
        private Lazy<StepsAndroid> android = new Lazy<StepsAndroid>();
        private Lazy<StepsiOS> iOs = new Lazy<StepsiOS>();

        public StepsCharMap CharMap => charmap.Value;

        public StepsYandexMarket YandexMarket => yandexMarket.Value;

        public StepsAndroid Android => android.Value;

        public StepsiOS IOS => iOs.Value;

        [TestStep("Check that '{0}' {1}")]
        public void CheckThat(IControl control, Matcher controlMatcher)
        {
            Assert.That(control, controlMatcher);
        }
    }
}
