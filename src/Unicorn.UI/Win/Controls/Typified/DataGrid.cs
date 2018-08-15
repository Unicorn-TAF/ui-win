using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class DataGrid : WinControl
    {
        public DataGrid()
        {
        }

        public DataGrid(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_DataGridControlTypeId;


        protected IUIAutomationGridPattern GridPattern => base.GetPattern(UIA_PatternIds.UIA_GridPatternId) as IUIAutomationGridPattern;
    }
}
