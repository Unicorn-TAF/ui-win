using System.Collections.Generic;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public enum DropdownElement
    {
        TextInput = 1,
        ExpandCollapse = 2,
        List = 3,
        ListItem = 4
    }

    public interface IDynamicDropdown : IDynamicControl, IDropdown
    {
        ITextInput Input { get; }

        IControl ExpandCollapse { get; }

        IControl ItemsContainer { get; }

        IList<T> GetItems<T>() where T : IControl;
    }
}
