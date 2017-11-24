namespace Unicorn.UI.Core.UI
{
    public interface ISelectable
    {
        bool MultiSelect { get; }

        string SelectedValue { get; }

        bool Select(string itemName);
    }
}
