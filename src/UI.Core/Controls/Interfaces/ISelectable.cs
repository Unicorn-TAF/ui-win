namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface ISelectable
    {
        string SelectedValue { get; }

        bool Select(string itemName);
    }
}
