using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POE
{
    /// <summary>
    /// Interaction logic for SpecificRecipe.xaml
    /// </summary>
    public partial class SpecificRecipe : Window
    {
        public SpecificRecipe()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event of the Search Recipe button.
        /// Searches for the recipe by name and navigates to MainWindow if found.
        /// Displays a message if the recipe is not found.
        /// </summary>
        private void btnSearchRecipe_Click(object sender, RoutedEventArgs e)
        {
            string searchRecipeName = txtSearchRecipe.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchRecipeName))
            {
                MessageBox.Show("Please enter a recipe name.", "Empty Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var foundRecipes = MainWindow.AllRecipes
                .Where(r => r.Name.Equals(searchRecipeName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (foundRecipes.Any())
            {
                MainWindow main = new MainWindow();
                main.Show();
                main.DisplaySpecificRecipes(foundRecipes);
                this.Close();
            }
            else
            {
                MessageBox.Show("The recipe does not exist or you must add the recipe.", "Recipe Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Handles the click event of the Cancel button.
        /// Closes the SpecificRecipe window and returns to MainWindow.
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
