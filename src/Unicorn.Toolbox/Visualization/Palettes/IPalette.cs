using System.Collections.Generic;
using System.Windows.Media;

namespace Unicorn.Toolbox.Visualization.Palettes
{
    public interface IPalette
    {
        Brush BackColor { get; }

        Brush FontColor { get; }

        List<Brush> DataColors { get; }

        Brush DataFontColor { get; }
    }
}
