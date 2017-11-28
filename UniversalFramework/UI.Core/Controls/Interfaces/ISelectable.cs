namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface ISelectable
    {
        bool MultiSelect { get; }

        string SelectedValue { get; }

        bool Select(string itemName);
    }
}
