namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface IItemSelectable
    {
        string SelectedValue
        {
            get;
        }

        bool Select(string itemName);
    }
}
