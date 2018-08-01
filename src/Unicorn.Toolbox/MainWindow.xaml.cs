using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unicorn.Toolbox.Analysis;
using Unicorn.Toolbox.Analysis.Filtering;
using Unicorn.Toolbox.Coverage;
using Unicorn.Toolbox.Visualization;

namespace Unicorn.Toolbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Analyzer analyzer;
        private SpecsCoverage coverage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLoadTestsAssembly_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Unicorn tests assemblies|*.dll";
            openFileDialog.ShowDialog();

            string assemblyName = openFileDialog.FileName;

            if (string.IsNullOrEmpty(assemblyName))
            {
                return;
            }

            var testsAssembly = Assembly.LoadFrom(assemblyName);
            this.analyzer = new Analyzer(testsAssembly, assemblyName);
            this.analyzer.GetTestsStatistics();

            var statusLine = $"Assembly: {this.analyzer.AssemblyFile} ({this.analyzer.AssemblyName})    |    " + this.analyzer.Data.ToString();
            this.textBoxStatistics.Text = statusLine;

            FillFiltersFrom(analyzer.Data);
            ShowAll();
        }

        private void FillFiltersFrom(AutomationData data)
        {
            FillGrid(gridFeatures, data.UniqueFeatures);
            FillGrid(gridCategories, data.UniqueCategories);
            FillGrid(gridAuthors, data.UniqueAuthors);
        }

        private void FillGrid(Grid grid, HashSet<string> items)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            for (int i = 0; i < 35; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            int index = 2;

            foreach (var item in items)
            {
                var itemCheckbox = new CheckBox();
                itemCheckbox.Content = item;
                itemCheckbox.IsChecked = true;
                grid.Children.Add(itemCheckbox);
                Grid.SetRow(itemCheckbox, index++);
            }
        }

        private void buttonApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            var features = from CheckBox cBox in gridFeatures.Children where cBox.IsChecked.Value select (string)cBox.Content;
            var categories = from CheckBox cBox in gridCategories.Children where cBox.IsChecked.Value select (string)cBox.Content;
            var authors = from CheckBox cBox in gridAuthors.Children where cBox.IsChecked.Value select (string)cBox.Content;

            this.analyzer.Data.ClearFilters();
            this.analyzer.Data.FilterBy(new FeaturesFilter(features));
            this.analyzer.Data.FilterBy(new CategoriesFilter(categories));
            this.analyzer.Data.FilterBy(new AuthorsFilter(authors));

            gridResults.ItemsSource = analyzer.Data.FilteredInfo;

            textBoxCurrentFilter.Text = $"Filter by:\nFeatures[{string.Join(",", features)}]\n";
            textBoxCurrentFilter.Text += $"Categories[{string.Join(",", categories)}]\n";
            textBoxCurrentFilter.Text += $"Authors[{string.Join(",", authors)}]";
        }

        private void cellSuiteName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                string testSuiteName = (sender as TextBlock).Text;

                var preview = new WindowTestPreview();
                preview.ShowActivated = false;
                preview.Show();
                preview.gridResults.ItemsSource = analyzer.Data.FilteredInfo.Where(s => s.Name.Equals(testSuiteName)).First().TestsInfos;
            }
        }

        private void buttonShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void buttonLoadSpecs_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Application specs|*.json";
            openFileDialog.ShowDialog();

            string specFileName = openFileDialog.FileName;

            if (string.IsNullOrEmpty(specFileName))
            {
                return;
            }

            this.coverage = new SpecsCoverage(specFileName);

            GetCoverage();
        }

        private void buttonGetCoverage_Click(object sender, RoutedEventArgs e)
        {
            GetCoverage();
        }

        private void buttonVisualize_Click(object sender, RoutedEventArgs e)
        {
            var filter = GetFilter();
            var visualization = new WindowVisualization();
            visualization.ShowActivated = false;
            visualization.Title = $"Overall tests statistics: {filter}";
            visualization.Show();

            if (checkBoxModern.IsChecked.HasValue && checkBoxModern.IsChecked.Value)
            {
                VisualizerCircles.VisualizeAllData(analyzer.Data, filter, visualization.canvasVisualization);
            }
            else
            {
                VizualizerBars.VisualizeAllData(analyzer.Data, filter, visualization.canvasVisualization);
            }
            
        }

        private void ShowAll()
        {
            this.analyzer.Data.ClearFilters();
            gridResults.ItemsSource = analyzer.Data.FilteredInfo;
        }

        private void GetCoverage()
        {
            this.coverage.Analyze(this.analyzer.Data.FilteredInfo);
            this.gridCoverage.ItemsSource = null;
            this.gridCoverage.ItemsSource = coverage.Specs.Modules;
        }

        private FilterType GetFilter()
        {
            if (tabFeaures.IsSelected)
            {
                return FilterType.Feature;
            }
            else if (tabCategories.IsSelected)
            {
                return FilterType.Category;
            }
            else
            {
                return FilterType.Author;
            }
        }

        private void buttonVisualizeCoverage_Click(object sender, RoutedEventArgs e)
        {
            var visualization = new WindowVisualization();
            visualization.ShowActivated = false;
            visualization.Title = "Modules coverage by tests";
            visualization.Show();

            if (checkBoxModern.IsChecked.HasValue && checkBoxModern.IsChecked.Value)
            {
                VisualizerCircles.VisualizeCoverage(coverage.Specs, visualization.canvasVisualization);
            }
            else
            {
                VizualizerBars.VisualizeCoverage(coverage.Specs, visualization.canvasVisualization);
            }
        }
    }
}
