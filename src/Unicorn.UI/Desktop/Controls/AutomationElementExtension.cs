using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls
{
    /// <summary>
    /// Extensions for automation element.
    /// </summary>
    public static class AutomationElementExtension
    {
        /// <summary>
        /// Get pattern of the specified type from the control.
        /// </summary>
        /// <typeparam name="T">pattern type</typeparam>
        /// <param name="element"></param>
        /// <returns>pattern instance</returns>
        public static T GetPattern<T>(this AutomationElement element) where T : BasePattern
        {
            var pattern = (AutomationPattern)typeof(T).GetField("Pattern").GetValue(null);
            object patternObject;
            element.TryGetCurrentPattern(pattern, out patternObject);
            return patternObject as T;
        }
    }
}
