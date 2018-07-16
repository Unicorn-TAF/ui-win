using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicorn.Toolbox.Analysis;
using Unicorn.Toolbox.Analysis.Filtering;
using Unicorn.Toolbox.Coverage;

namespace Unicorn.Toolbox.Visualization
{
    public class Visualizer
    {
        private static Random random = new Random();
        private static int margin = 30;
        private static List<Rect> rects = new List<Rect>();

        public static void VisualizeAllData(AutomationData data, FilterType filterType, Canvas canvas)
        {
            rects.Clear();
            canvas.Children.Clear();

            var stats = GetStats(data, filterType);

            int max = stats.Values.Max();
            int featuresCount = stats.Values.Count;

            foreach (var key in stats.Keys)
            {
                int radius = CalculateRadius(stats[key], max, featuresCount, (int)canvas.RenderSize.Width);
                DrawFeature(key, stats[key], radius, canvas);
            }
        }

        public static void VisualizeCoverage(AppSpecs specs, Canvas canvas)
        {
            rects.Clear();
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

            foreach (var key in featuresStats.Keys)
            {
                int radius = CalculateRadius(featuresStats[key], max, featuresCount, (int)canvas.RenderSize.Width);
                DrawFeature(key, featuresStats[key], radius, canvas);
            }
        }

        private static Dictionary<string, int> GetStats(AutomationData data, FilterType filterType)
        {
            var stats = new Dictionary<string, int>();

            switch (filterType)
            {
                case FilterType.Feature:
                    {
                        foreach (var feature in data.UniqueFeatures)
                        {
                            var suites = data.SuitesInfos.Where(s => s.Features.Contains(feature));
                            var tests = from SuiteInfo s
                                        in suites
                                        select s.TestsInfos;

                            stats.Add(feature, tests.Sum(t => t.Count));
                        }
                        return stats;
                    }
                case FilterType.Category:
                    {
                        foreach (var category in data.UniqueCategories)
                        {
                            var suites = data.SuitesInfos.Where(s => s.TestsInfos.Where(t => t.Categories.Contains(category)).Any());
                            var tests = from SuiteInfo s
                                        in suites
                                        select s.TestsInfos;

                            stats.Add(category, tests.Sum(t => t.Count));
                        }
                        return stats;
                    }
                case FilterType.Author:
                    {
                        foreach (var author in data.UniqueAuthors)
                        {
                            var suites = data.SuitesInfos.Where(s => s.TestsInfos.Where(t => t.Author.Equals(author)).Any());
                            var tests = from SuiteInfo s
                                        in suites
                                        select s.TestsInfos;

                            stats.Add(author, tests.Sum(t => t.Count));
                        }
                        return stats;
                    }
            }

            throw new ArgumentException("please check args");
        }

        private static void DrawFeature(string name, int tests, int radius, Canvas canvas)
        {
            int x = 0;
            int y = 0;
            Rect rect;

            do
            {
                x = random.Next(margin + radius, (int)canvas.RenderSize.Width - radius - margin);
                y = random.Next(margin + radius, (int)canvas.RenderSize.Height - radius - margin);

                rect = new Rect(x - radius - margin, y - radius - margin, (radius + margin) * 2, (radius + margin) * 2);
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
            label.Text = $"{name}\n{tests} tests";
            label.TextAlignment = TextAlignment.Center;
            label.Foreground = Brushes.White;

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
            if (capacity == 0)
            {
                return 1;
            }

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
