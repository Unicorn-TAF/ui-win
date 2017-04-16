using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UICore.UI
{
    public interface ISelectable
    {
        bool IsSelected
        {
            get;
        }

        bool Select();
    }
}
