namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface IMultiItemSelectable
    {
        string[] SelectedValues
        {
            get;
        }

        bool SelectMultiple(params string[] itemName);
    }
}
