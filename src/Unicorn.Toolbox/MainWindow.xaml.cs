using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unicorn.Toolbox.Analysis;

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

            this.textBoxStatistics.Text = "";
            this.textBoxStatistics.Text = this.analyzer.GetTestsStatistics();

            FillFiltersFrom(analyzer.Data);
        }

        private void FillFiltersFrom(AutomationData data)
        {
            canvasFilters.Children.Clear();

            var labelFeatures = new Label();
            labelFeatures.Content = "AVAILABLE FEATURES";
            canvasFilters.Children.Add(labelFeatures);
            Canvas.SetTop(labelFeatures, 10);
            Canvas.SetLeft(labelFeatures, 5);

            int featureIndex = 1;
            foreach (var feature in data.UniqueFeatures)
            {
                var featureControl = new CheckBox();
                featureControl.Content = feature;

                canvasFilters.Children.Add(featureControl);
                Canvas.SetTop(featureControl, 20 + 20 * featureIndex++);
                Canvas.SetLeft(featureControl, 20);
            }


            int categoriesOffset = 30 + 20 * featureIndex;

            var labelategories = new Label();
            labelategories.Content = "AVAILABLE CATEGORIES";
            canvasFilters.Children.Add(labelategories);
            Canvas.SetTop(labelategories, categoriesOffset);
            Canvas.SetLeft(labelategories, 5);


            int categoryIndex = 1;
            foreach (var category in data.UniqueCategories)
            {
                var categoryControl = new CheckBox();
                categoryControl.Content = category;

                canvasFilters.Children.Add(categoryControl);
                Canvas.SetTop(categoryControl, 10 + categoriesOffset + 20 * categoryIndex++);
                Canvas.SetLeft(categoryControl, 20);
            }
        }

        private void buttonApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            //var groups = (this.Resources["CvsKey"] as CollectionViewSource).View.Groups;
            gridResults.ItemsSource = analyzer.Data.SuitesInfos;
        }

        private void cellSuiteName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                string testSuiteName = (sender as TextBlock).Text;

                var preview = new WindowTestPreview();
                preview.Show();
                preview.gridResults.ItemsSource = analyzer.Data.SuitesInfos.Where(s => s.Name.Equals(testSuiteName)).First().TestsInfos;
                preview.Focus();
            }
        }
    }
}
