using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POE
{
    public partial class pieChart : Window
    {
        public SeriesCollection PieChartData { get; set; }

        /// <summary>
        /// Initializes a new instance of the pieChart class. Sets up the pie chart with data from selected recipes.
        /// </summary>
        public pieChart(List<Recipe> selectedRecipes)
        {
            InitializeComponent();

            PieChartData = new SeriesCollection();
            CalculateFoodGroupPercentages(selectedRecipes);
            DataContext = this;
        }

        /// <summary>
        /// Calculates the percentages of each food group in the selected recipes and adds them to the pie chart.
        /// </summary>
        private void CalculateFoodGroupPercentages(List<Recipe> selectedRecipes)
        {
            var foodGroupCounts = new Dictionary<string, double>();

            foreach (var recipe in selectedRecipes)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (foodGroupCounts.ContainsKey(ingredient.FoodGroup))
                    {
                        foodGroupCounts[ingredient.FoodGroup] += ingredient.Quantity;
                    }
                    else
                    {
                        foodGroupCounts[ingredient.FoodGroup] = ingredient.Quantity;
                    }
                }
            }

            double totalIngredients = foodGroupCounts.Values.Sum();
            foreach (var fg in foodGroupCounts)
            {
                double percentage = Math.Round((fg.Value / totalIngredients) * 100, 2);
                PieChartData.Add(new PieSeries
                {
                    Title = fg.Key,
                    Values = new ChartValues<double> { percentage },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Y:F2}%"
                });
            }

            pieChartView.Series = PieChartData;
        }

        /// <summary>
        /// Calculates the percentages of each food group in the selected recipes and adds them to the pie chart.
        /// </summary>
        // Remove or adjust if needed
        private void btnPieChart_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Handles the ContextMenuClosing event of the pie chart view.
        /// </summary>
        private void pieChartView_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            
        }

        /// <summary>
        /// Handles the click event of the Home button. Returns to the main window.
        /// </summary>
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
