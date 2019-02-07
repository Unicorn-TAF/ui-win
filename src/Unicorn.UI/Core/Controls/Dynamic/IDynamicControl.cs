using System;
using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public interface IDynamicControl<T> where T : IConvertible
    {
        void Populate(Dictionary<T, ByLocator> elementsLocators);
    }
}
