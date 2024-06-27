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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Recipe> AllRecipes { get; set; } = new List<Recipe>();

        /// <summary>
        /// Initializes the main window, updates the recipe list box, and adds default recipes.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            AddPremadeRecipes();
            UpdateRecipesListBox();
            this.Loaded += MainWindow_Loaded;
            lstboxAllRecipes.SelectionChanged += LstboxAllRecipes_SelectionChanged; 
        }

        /// <summary>
        /// Handles the Loaded event of the MainWindow. Checks if there are any recipes and shows a prompt if the list is empty.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Check if there are no recipes and show a prompt if the list is empty
            if (AllRecipes.Count == 0)
            {
                MessageBox.Show("Please add a recipe if you want to proceed with the app!", "Senele's Recipe App", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            UpdateRecipesListBox();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the recipe ListBox. Displays details of the selected recipe.
        /// </summary>
        private void LstboxAllRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstboxAllRecipes.SelectedItem is Recipe selectedRecipe)
            {
                DisplayRecipeDetails(selectedRecipe);
            }
        }

        /// <summary>
        /// Displays the details of the selected recipe in a message box.
        /// </summary>
        private void DisplayRecipeDetails(Recipe recipe)
        {
            // Create a new window or use a text block in the main window to display recipe details
            MessageBox.Show(recipe.ToString(), "Recipe Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the click event of the Add button.
        /// Opens a new AddRecipe window to allow the user to add a new recipe.
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe add = new AddRecipe();
            add.Show();
            this.Close();
            UpdateRecipesListBox();
        }

        /// <summary>
        /// Handles the click event of the Display All button. Displays all recipes in the list box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void btnDisplayAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateRecipesListBox();
        }

        /// <summary>
        /// Displays the specific recipes in the list box.
        /// </summary>
        /// <param name="recipes">The list of recipes to display.</param>
        private void btnDisplaySpecific_Click(object sender, RoutedEventArgs e)
        {
            SpecificRecipe spec = new SpecificRecipe();
            spec.Show();
            this.Close();
        }

        /// <summary>
        /// Displays the specific recipes in the list box.
        /// </summary>
        /// <param name="recipes">The list of recipes to display.</param>
        public void DisplaySpecificRecipes(List<Recipe> recipes)
        {
            lstboxAllRecipes.Items.Clear();
            foreach (var recipe in recipes)
            {
                lstboxAllRecipes.Items.Add(recipe.ToString());
            }
        }

        /// <summary>
        /// Handles the click event of the Reset button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void btnScale_Click(object sender, RoutedEventArgs e)
        {
            if (AllRecipes.Count == 0)
            {
                MessageBox.Show("Please add a recipe before scaling.");
                return;
            }

            Recipe selectedRecipe;

            if (lstboxAllRecipes.SelectedItem != null)
            {
                selectedRecipe = lstboxAllRecipes.SelectedItem as Recipe;
            }
            else if (AllRecipes.Count == 1)
            {
                selectedRecipe = AllRecipes[0];
            }
            else
            {
                MessageBox.Show("Please select a recipe to scale.");
                return;
            }

            Scale scaleWindow = new Scale(selectedRecipe);
            scaleWindow.ShowDialog();

            // Update the list box after scaling
            UpdateRecipesListBox();
        }

        /// <summary>
        /// Handles the click event of the Pie Chart button. Creates a pie chart of food groups for selected recipes.
        /// </summary>
        private void btnPie_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecipes = lstboxAllRecipes.SelectedItems.Cast<Recipe>().ToList();
            if (selectedRecipes.Count == 0)
            {
                MessageBox.Show("Please select at least one recipe to create a menu.");
                return;
            }

            pieChart chart = new pieChart(selectedRecipes);
            chart.Show();
            this.Close();
        }

        /// <summary>
        /// Handles the click event of the Clear button. Clears all recipes from the list box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // Show a confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to clear all recipes, including premade ones? This action cannot be undone.",
                "Confirm Clear All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Clear all recipes
                AllRecipes.Clear();
                UpdateRecipesListBox();
                MessageBox.Show("All recipes have been cleared.", "Recipes Cleared", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Updates the recipes list box with the current contents of AllRecipes.
        /// </summary>
        public void UpdateRecipesListBox()
        {
            lstboxAllRecipes.Items.Clear();

            // Sort recipes alphabetically by name
            var sortedRecipes = AllRecipes.OrderBy(r => r.Name).ToList();

            foreach (var recipe in sortedRecipes)
            {
                lstboxAllRecipes.Items.Add(recipe);
            }
        }

        /// <summary>
        /// Handles the click event of the Reset button. Resets the selected recipe to its original state.
        /// </summary>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Recipe selectedRecipe = (Recipe)lstboxAllRecipes.SelectedItem;
            if (selectedRecipe != null)
            {
                selectedRecipe.ResetToOriginal();
                UpdateRecipesListBox();
            }
            else
            {
                MessageBox.Show("Please select a recipe to reset.", "No Recipe Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Created 2 premade recipes so if the user does not want to add a recipe,
        /// they can just use the premade ones.
        /// </summary>
        private void AddPremadeRecipes()
        {
            if (!AllRecipes.Any(r => r.Name == "Spaghetti Bolognese" || r.Name == "Chocolate Chip Cookies"))
            {
                // Spaghetti Bolognese
                Recipe spaghetti = new Recipe
                {
                    Name = "Spaghetti Bolognese",
                    Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Spaghetti", Quantity = 400, UnitOfMeasure = "g", Calories = 632, FoodGroup = "Grains" },
                new Ingredient { Name = "Ground Beef", Quantity = 500, UnitOfMeasure = "g", Calories = 1660, FoodGroup = "Proteins" },
                new Ingredient { Name = "Tomato Sauce", Quantity = 400, UnitOfMeasure = "ml", Calories = 118, FoodGroup = "Vegetables" },
                new Ingredient { Name = "Onion", Quantity = 1, UnitOfMeasure = "piece", Calories = 40, FoodGroup = "Vegetables" },
                new Ingredient { Name = "Garlic", Quantity = 2, UnitOfMeasure = "cloves", Calories = 9, FoodGroup = "Vegetables" },
                new Ingredient { Name = "Olive Oil", Quantity = 2, UnitOfMeasure = "tbsp", Calories = 238, FoodGroup = "Oils & Solid Fats" }
            },
                    Steps = new List<string>
            {
                "Cook spaghetti according to package instructions.",
                "In a large pan, heat olive oil and sauté onions and garlic.",
                "Add ground beef and cook until browned.",
                "Add tomato sauce and simmer for 20 minutes.",
                "Serve sauce over cooked spaghetti."
            }
                };

                // Chocolate Chip Cookies
                Recipe cookies = new Recipe
                {
                    Name = "Chocolate Chip Cookies",
                    Ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Flour", Quantity = 250, UnitOfMeasure = "g", Calories = 910, FoodGroup = "Grains" },
                new Ingredient { Name = "Butter", Quantity = 200, UnitOfMeasure = "g", Calories = 1434, FoodGroup = "Diary" },
                new Ingredient { Name = "Sugar", Quantity = 200, UnitOfMeasure = "g", Calories = 774, FoodGroup = "Added Sugars" },
                new Ingredient { Name = "Eggs", Quantity = 2, UnitOfMeasure = "pieces", Calories = 155, FoodGroup = "Proteins" },
                new Ingredient { Name = "Chocolate Chips", Quantity = 200, UnitOfMeasure = "g", Calories = 958, FoodGroup = "Added Sugars" },
                new Ingredient { Name = "Vanilla Extract", Quantity = 1, UnitOfMeasure = "tsp", Calories = 12, FoodGroup = "Others" }
            },
                    Steps = new List<string>
            {
                "Cream together butter and sugar.",
                "Beat in eggs and vanilla extract.",
                "Gradually mix in flour.",
                "Fold in chocolate chips.",
                "Drop spoonfuls of dough onto a baking sheet.",
                "Bake at 180°C for 10-12 minutes."
            }
                };

                AllRecipes.Add(spaghetti);
                AllRecipes.Add(cookies);
            }
        }
    }
}