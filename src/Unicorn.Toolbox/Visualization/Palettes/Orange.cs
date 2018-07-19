using System;
using System.Collections.Generic;
using System.Windows.Media;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class Orange : IPalette
    {
        public Orange()
        {
            dataColors = new List<Brush>();
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF6C00")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F57C00")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8C00")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA726")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB74D")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCC80")));
            dataColors.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE0B2")));
        }

        private Brush backColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3E0"));

        private Brush fontColor = Brushes.Black;

        private Brush dataColor = Brushes.Black;

        private List<Brush> dataColors;

        public Brush BackColor => backColor;

        public List<Brush> Colors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<Brush> DataColors => dataColors;

        public Brush DataFontColor => dataColor;

        public Brush FontColor => fontColor;
    }
}
