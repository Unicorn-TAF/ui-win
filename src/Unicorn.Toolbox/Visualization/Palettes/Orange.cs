using System.Collections.Generic;
using System.Windows.Media;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class Orange : IPalette
    {
        public Orange()
        {
            DataColors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF6C00")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F57C00")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8C00")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA726")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB74D")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCC80")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE0B2"))
            };
        }

        public Brush BackColor { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3E0"));

        public List<Brush> DataColors { get; protected set; }

        public Brush DataFontColor { get; } = Brushes.Black;

        public Brush FontColor { get; } = Brushes.Black;
    }
}
