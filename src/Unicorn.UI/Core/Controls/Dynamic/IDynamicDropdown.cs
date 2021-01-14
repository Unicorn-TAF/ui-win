using System.Collections.Generic;
using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public enum DropdownElement
    {
        ValueInput = 1,
        ExpandCollapse = 2,
        OptionsFrame = 3,
        Option = 4,
        Loader = 5
    }

    public interface IDynamicDropdown : IDynamicControl, IDropdown, ILoadable
    {
        ITextInput ValueInput { get; }

        IControl ExpandCollapse { get; }

        IControl OptionsFrame { get; }

        IList<IControl> GetOptions();

        IControl GetOption(string optionName);

        void SearchFor(string optionName);
    }
}
