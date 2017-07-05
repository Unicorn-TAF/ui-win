namespace Unicorn.UICore.UI
{
    public interface IContainer
    {
        void ClickButton(string buttonName);

        bool SelectRadio(string radioLabel);

        bool SetheckboxState(string checkboxLabel, bool state);

        bool InputText(string inputName, string text);
    }
}
