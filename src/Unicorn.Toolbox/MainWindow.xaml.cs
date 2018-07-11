using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unicorn.Toolbox.Analysis;
using Unicorn.Toolbox.Analysis.Filtering;

namespace Unicorn.Toolbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Analyzer analyzer;

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

            var featuresFilter = new FeaturesFilter(features);
            var categoriesFilter = new CategoriesFilter(categories);
            var authorsFilter = new AuthorsFilter(authors);

            analyzer.Data.FilteredInfo = featuresFilter.FilterSuites(analyzer.Data.SuitesInfos);
            analyzer.Data.FilteredInfo = categoriesFilter.FilterSuites(analyzer.Data.FilteredInfo);
            analyzer.Data.FilteredInfo = authorsFilter.FilterSuites(analyzer.Data.FilteredInfo);
            gridResults.ItemsSource = analyzer.Data.FilteredInfo;

            textBoxCurrentFilter.Text = $"Filter by: Features[{string.Join(",", features)}] Categories[{string.Join(",", categories)}] Authors[{string.Join(",", authors)}]";
        }

        private void cellSuiteName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                string testSuiteName = (sender as TextBlock).Text;

                var preview = new WindowTestPreview();
                preview.ShowActivated = false;
                preview.Show();
                preview.gridResults.ItemsSource = analyzer.Data.SuitesInfos.Where(s => s.Name.Equals(testSuiteName)).First().TestsInfos;
            }
        }

        private void buttonShowAll_Click(object sender, RoutedEventArgs e)
        {
            gridResults.ItemsSource = analyzer.Data.SuitesInfos;
        }
    }
}
