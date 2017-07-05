namespace Unicorn.UICore.UI
{
    public interface ISelectable
    {
        bool IsSelected
        {
            get;
        }

        bool Select();
    }
}
