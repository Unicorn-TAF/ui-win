namespace Unicorn.UICore.UI.Controls
{
    public interface ITextInput
    {
        string Value
        {
            get;
        }

        void SendKeys(string text);
    }
}
