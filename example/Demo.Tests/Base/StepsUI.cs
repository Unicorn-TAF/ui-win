using System;
using Demo.Celestia.Steps;
using Demo.Charmap.Steps;
using Demo.AndroidDialer.Steps;

namespace Demo.Tests.Base
{
    public class StepsUI
    {
        private readonly Lazy<StepsCharMap> _charmap = new Lazy<StepsCharMap>();
        private readonly Lazy<StepsCelestia> _celestia = new Lazy<StepsCelestia>();
        private readonly Lazy<StepsAndroidDialer> _android = new Lazy<StepsAndroidDialer>();

        public StepsCharMap CharMap => _charmap.Value;

        public StepsCelestia Celestia => _celestia.Value;

        public StepsAndroidDialer Android => _android.Value;

    }
}
