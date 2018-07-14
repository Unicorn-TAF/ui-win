using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

            for (int i = 0; i < 25; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            int index = 1;

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
            this.analyzer.Data.ClearFilters();
            gridResults.ItemsSource = analyzer.Data.FilteredInfo;
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
        }

        private void buttonGetCoverage_Click(object sender, RoutedEventArgs e)
        {
            this.coverage.Analyze(this.analyzer.Data.FilteredInfo);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Coverage:");

            var covered = from Coverage.Module m
                          in this.coverage.Specs.Modules.Where(m => m.Suites.Any())
                          select m.Name;

            sb.Append("Covered modules: " + string.Join(",", covered));
            sb.AppendLine();

            var notCovered = from Coverage.Module m
                          in this.coverage.Specs.Modules.Where(m => !m.Suites.Any())
                             select m.Name;

            sb.Append("Not covered modules: " + string.Join(",", notCovered));
            sb.AppendLine();

            this.textBoxCoverage.Text = sb.ToString();
        }

        private void buttonVisualize_Click(object sender, RoutedEventArgs e)
        {
            var visualization = new WindowVisualization();
            visualization.ShowActivated = false;
            visualization.Show();

            Visualizer.VisualizeCoverage(analyzer.Data, visualization.canvasVisualization);
        }
    }
}
