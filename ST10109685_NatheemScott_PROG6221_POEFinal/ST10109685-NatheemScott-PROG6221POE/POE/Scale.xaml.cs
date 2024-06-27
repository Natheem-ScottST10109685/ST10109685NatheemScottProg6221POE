using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POE
{
    public partial class Scale : Window
    {
        private Recipe recipeToScale;

        /// <summary>
        /// Initializes a new instance of the Scale class. Sets up the window for scaling a recipe.
        /// </summary>
        public Scale(Recipe recipe)
        {
            InitializeComponent();
            recipeToScale = recipe;
        }

        /// <summary>
        /// Handles the click event of the Confirm button. Applies the selected scaling factor to the recipe.
        /// </summary>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (cbScalingFactor.SelectedItem == null)
            {
                MessageBox.Show("Please select a scaling factor.");
                return;
            }

            string scalingFactorString = (cbScalingFactor.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            if (string.IsNullOrEmpty(scalingFactorString))
            {
                MessageBox.Show("Error: Invalid scaling factor selection.");
                return;
            }

            if (!double.TryParse(scalingFactorString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double scalingFactor))
            {
                MessageBox.Show($"Error: Unable to parse scaling factor '{scalingFactorString}'.");
                return;
            }

            if (scalingFactor <= 0)
            {
                MessageBox.Show("Error: Scaling factor must be greater than zero.");
                return;
            }

            recipeToScale.ScaleRecipe(scalingFactor);

            // Notify the main window about the update
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.UpdateRecipesListBox();

            this.Close();
        }

        /// <summary>
        /// Scales the given recipe by the specified factor and displays the updated recipe details.
        /// </summary>
        private void ScaleRecipe(Recipe recipe, double scalingFactor)
        {
            // Make a copy of the original recipe
            Recipe originalRecipe = new Recipe
            {
                Name = recipe.Name,
                Ingredients = new System.Collections.Generic.List<Ingredient>(recipe.Ingredients),
                Steps = new System.Collections.Generic.List<string>(recipe.Steps)
            };

            // Scale the ingredients and round to 1 decimal place
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.Quantity = Math.Round(ingredient.Quantity * scalingFactor, 1);
                ingredient.Calories = Math.Round(ingredient.Calories * scalingFactor, 1);
            }

            // Show a message box with scaled details
            MessageBox.Show($"Recipe scaled by {scalingFactor}. Updated values:\n{recipe}");
        }
    }
}