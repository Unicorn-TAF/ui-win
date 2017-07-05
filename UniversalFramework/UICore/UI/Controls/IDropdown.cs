namespace Unicorn.UICore.UI.Controls
{
    public interface IDropdown
    {
        bool isExpanded
        {
            get;
        }

        string SelectedValue
        {
            get;
        }

        bool Select(string item);

        void CheckItems(string[] items);
    }
}
