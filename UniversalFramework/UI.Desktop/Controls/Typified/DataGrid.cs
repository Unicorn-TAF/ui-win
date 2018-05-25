using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class DataGrid : GuiControl
    {
        public DataGrid()
        {
        }

        public DataGrid(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.DataGrid;

        protected GridPattern GridPattern => GetPattern<GridPattern>();
    }
}
