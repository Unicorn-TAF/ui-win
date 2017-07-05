namespace Unicorn.UICore.UI.Controls
{
    public interface ITreeView
    {
        bool IsMulti
        {
            get;
        }

        bool Select(string item);
    }
}
