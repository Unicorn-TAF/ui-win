using System;
using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public interface IDynamicControl
    {
        void Populate(Dictionary<int, ByLocator> elementsLocators);
    }
}
