using System.Collections.Generic;
using System.Windows.Media;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class LightGreen : IPalette
    {
        public LightGreen()
        {
            DataColors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#558B2F")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#689F38")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7CB342")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8BC34A")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9CCC65")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AED581")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C5E1A5")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCEDC8")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F8E9"))
            };
        }

        public Brush BackColor { get; } = Brushes.White;

        public List<Brush> DataColors { get; protected set; }

        public Brush DataFontColor { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111"));

        public Brush FontColor { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111"));
    }
}
