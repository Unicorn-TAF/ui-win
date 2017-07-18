namespace Unicorn.UICore.UI.Controls
{
    public interface ICheckbox
    {
        bool Checked
        {
            get;
        }

        bool Check();

        bool Uncheck();
        
    }
}
