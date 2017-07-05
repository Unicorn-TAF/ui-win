namespace Unicorn.UICore.UI.Controls
{
    public interface IWindow : IContainer
    {
        string Title
        {
            get;
        }

        void Close();
    }
}
