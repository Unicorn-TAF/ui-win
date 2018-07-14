using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicorn.Toolbox.Analysis;

namespace Unicorn.Toolbox.Visualization
{
    public class Visualizer
    {
        private static Random random = new Random();
        private static int margin = 10;
        private static List<Rect> rects = new List<Rect>();

        public static void VisualizeCoverage(AutomationData data, Canvas canvas)
        {
            rects.Clear();
            canvas.Children.Clear();
            canvas.Background = Brushes.DarkGray;

            var featuresStats = new Dictionary<string, int>();

            foreach (var feature in data.UniqueFeatures)
            {
                var suites = data.SuitesInfos.Where(s => s.Features.Contains(feature));
                var tests = from SuiteInfo s
                            in suites
                            select s.TestsInfos;

                featuresStats.Add(feature, tests.Count());
            }

            int max = featuresStats.Values.Max();
            int featuresCount = featuresStats.Values.Count;

            foreach (var key in featuresStats.Keys)
            {
                int radius = CalculateRadius(featuresStats[key], max, featuresCount, (int)canvas.RenderSize.Width);
                DrawFeature(key, featuresStats[key], radius, canvas);
            }
        }

        private static void DrawFeature(string featureName, int tests, int radius, Canvas canvas)
        {
            int x = 0;
            int y = 0;
            Rect rect;

            do
            {
                x = random.Next(margin + radius, (int)canvas.RenderSize.Width - radius - margin);
                y = random.Next(margin + radius, (int)canvas.RenderSize.Height - radius - margin);

                rect = new Rect(x - radius, y - radius, radius * 2, radius * 2);
            }
            while (rects.Any(r => r.IntersectsWith(rect)));

            rects.Add(rect);

            var ellipse = new Ellipse()
            {
                Fill = GetRandomColor(),
                Width = radius * 2,
                Height = radius * 2
            };

            Canvas.SetLeft(ellipse, x - radius);
            Canvas.SetTop(ellipse, y - radius);
            canvas.Children.Add(ellipse);

            var label = new TextBlock();
            label.Text = $"{featureName}\n{tests} tests";
            label.TextAlignment = TextAlignment.Center;

            var formattedText = new FormattedText(
                label.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch),
                label.FontSize,
                Brushes.Black,
                new NumberSubstitution(), TextFormattingMode.Display);

            canvas.Children.Add(label);
            Canvas.SetLeft(label, x - formattedText.Width / 2);
            Canvas.SetTop(label, y - formattedText.Height / 2);
        }

        private static int CalculateRadius(int capacity, int max, int count, int canvasSize)
        {
            double radius = (double)canvasSize / (Math.Sqrt(count + margin));
            double ratio = (double)capacity / (double)max;
            return (int)(radius * ratio / 2);
        }

        private static Brush GetRandomColor()
        {
            return new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255),
              (byte)random.Next(1, 255), (byte)random.Next(1, 233)));
        }
    }
}
