using System.Collections.Generic;
using System.Windows.Media;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class DeepPurple : IPalette
    {
        private Brush backColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDE7F6"));

        private Brush fontColor = Brushes.Black;

        private Brush dataColor = Brushes.WhiteSmoke;

        private List<Brush> dataColors;

        public DeepPurple()
        {
            dataColors = new List<Brush>();
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#311B92")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4527A0")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#512DA8")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5E35B1")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#673AB7")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7E57C2")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9575CD")));
        }

        public Brush BackColor => backColor;

        public List<Brush> DataColors => dataColors;

        public Brush DataFontColor => dataColor;

        public Brush FontColor => fontColor;
    }
}
