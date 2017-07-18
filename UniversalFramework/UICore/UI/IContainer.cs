namespace Unicorn.UICore.UI
{
    public interface IContainer
    {
        void ClickButton(string locator);

        bool SelectRadio(string locator);

        bool SetCheckbox(string locator, bool state);

        void InputText(string locator, string text);
    }
}
