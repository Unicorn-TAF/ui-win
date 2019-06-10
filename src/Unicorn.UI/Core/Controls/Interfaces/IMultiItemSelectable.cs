using System.Collections.Generic;

namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface IMultiItemSelectable
    {
        List<string> SelectedValues
        {
            get;
        }

        bool SelectMultiple(params string[] itemName);
    }
}
