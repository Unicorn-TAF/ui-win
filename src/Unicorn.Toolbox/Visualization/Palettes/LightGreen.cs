using System.Collections.Generic;
using System.Windows.Media;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class LightGreen : IPalette
    {
        private Brush backColor = Brushes.White;

        private Brush fontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111"));

        private Brush dataColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111"));

        private List<Brush> dataColors;

        public LightGreen()
        {
            dataColors = new List<Brush>();
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#558B2F")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#689F38")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7CB342")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8BC34A")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9CCC65")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AED581")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C5E1A5")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCEDC8")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F8E9")));
        }

        public Brush BackColor => backColor;

        public List<Brush> DataColors => dataColors;

        public Brush DataFontColor => dataColor;

        public Brush FontColor => fontColor;
    }
}
