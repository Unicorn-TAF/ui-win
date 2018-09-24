namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    public interface ICheckbox
    {
        bool Checked
        {
            get;
        }

        bool SetCheckState(bool isChecked);
    }
}
