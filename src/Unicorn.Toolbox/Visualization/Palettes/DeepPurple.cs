using System.Collections.Generic;
using System.Windows.Media;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class DeepPurple : IPalette
    {
        public DeepPurple()
        {
            DataColors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#311B92")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4527A0")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#512DA8")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5E35B1")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#673AB7")),
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7E57C2")),
                (new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9575CD")))
            };
        }

        public Brush BackColor { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDE7F6"));

        public List<Brush> DataColors { get; protected set; }

        public Brush DataFontColor { get; } = Brushes.WhiteSmoke;

        public Brush FontColor { get; } = Brushes.Black;
    }
}
