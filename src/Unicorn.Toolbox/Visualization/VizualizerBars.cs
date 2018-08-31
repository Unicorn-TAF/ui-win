using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Unicorn.Toolbox.Analysis;
using Unicorn.Toolbox.Analysis.Filtering;
using Unicorn.Toolbox.Coverage;
using Unicorn.Toolbox.Visualization.Palettes;

namespace Unicorn.Toolbox.Visualization
{
    public class VizualizerBars
    {
        private static Random random = new Random();
        private static int margin = 15;

        private static IPalette palette = new LightGreen();

        public static void VisualizeAllData(AutomationData data, FilterType filterType, Canvas canvas)
        {
            canvas.Background = palette.BackColor;
            canvas.Children.Clear();

            var stats = VisualizerCircles.GetStats(data, filterType);

            int max = stats.Values.Max();
            int featuresCount = stats.Values.Count;

            var items = from pair in stats
                        orderby pair.Value descending
                        select pair;

            int currentIndex = 0;

            foreach (KeyValuePair<string, int> pair in items)
            {
                DrawFeature(pair.Key, pair.Value, currentIndex++, max, featuresCount, canvas);
            }
        }

        public static void VisualizeCoverage(AppSpecs specs, Canvas canvas)
        {
            canvas.Background = palette.BackColor;
            canvas.Children.Clear();

            var featuresStats = new Dictionary<string, int>();

            foreach (var module in specs.Modules)
            {
                var tests = from SuiteInfo s
                            in module.Suites
                            select s.TestsInfos;

                featuresStats.Add(module.Name, tests.Sum(t => t.Count));
            }

            int max = featuresStats.Values.Max();
            int featuresCount = featuresStats.Values.Count;

            var items = from pair in featuresStats
                        orderby pair.Value descending
                        select pair;

            int currentIndex = 0;

            foreach (KeyValuePair<string, int> pair in items)
            {
                DrawFeature(pair.Key, pair.Value, currentIndex++, max, featuresCount, canvas);
            }
        }

        private static void DrawFeature(string name, int tests, int index, int max, int featuresCount, Canvas canvas)
        {
            var workHeight = canvas.RenderSize.Height - (2 * margin);
            var workWidth = canvas.RenderSize.Width - (2 * margin);

            double height = (workHeight / featuresCount) - margin;
            double width = tests == 0 ? 1 : workWidth * ((double)tests / max); 

            double x = margin;
            double y = margin + (index * (height + margin));

            double colorIndexStep = (double)palette.DataColors.Count / featuresCount;
            int currentColorIndex = (int)(((index + 1) * colorIndexStep) - 1);

            var bar = new Rectangle()
            {
                Fill = palette.DataColors[currentColorIndex],
                Width = width,
                Height = height,
                StrokeThickness = 1,
                Stroke = Brushes.Black,
                Effect = new DropShadowEffect()
            };

            Canvas.SetLeft(bar, x);
            Canvas.SetTop(bar, y);
            canvas.Children.Add(bar);

            var label = new TextBlock();
            label.Text = $"{name}: {tests}";
            label.TextAlignment = TextAlignment.Center;
            label.FontFamily = new FontFamily("Calibri");
            label.FontSize = 15;

            var formattedText = new FormattedText(
                label.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch),
                label.FontSize,
                label.Foreground,
                new NumberSubstitution(), 
                TextFormattingMode.Display);

            label.Foreground = formattedText.Width > width ? palette.FontColor : palette.DataFontColor;

            canvas.Children.Add(label);
            Canvas.SetLeft(label, x + 2);
            Canvas.SetTop(label, y + 2);
        }
    }
}
