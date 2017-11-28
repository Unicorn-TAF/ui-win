namespace Unicorn.UI.Core.PageObject
{
    public interface IContainer
    {
        void Init();

        void ClickButton(string locator);

        bool SelectRadio(string locator);

        bool SetCheckbox(string locator, bool state);

        void InputText(string locator, string text);
    }
}
