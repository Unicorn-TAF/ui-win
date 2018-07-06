namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface IMultiSelectable
    {
        string[] SelectedValues { get; }

        bool SelectMultiple(params string[] itemName);
    }
}
