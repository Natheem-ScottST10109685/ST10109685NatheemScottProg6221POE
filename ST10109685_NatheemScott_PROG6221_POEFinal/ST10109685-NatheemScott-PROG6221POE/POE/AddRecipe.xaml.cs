using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POE
{
    /// <summary>
    /// Interaction logic for AddRecipe.xaml
    /// </summary>
    public partial class AddRecipe : Window
    {
        /// <summary>
        /// Constructor for the AddRecipe window.
        /// Initializes the components and sets up the initial state of the window.
        /// Adds food groups to the ComboBox and initializes a new Recipe object.
        /// </summary>        
        private Recipe currentRecipe;

        private bool isClearing = false;

        public AddRecipe()
        {
            InitializeComponent();
            // Adding food groups to the ComboxBox
            cmbboxFGroups.Items.Add("Vegatables");
            cmbboxFGroups.Items.Add("Fruits");
            cmbboxFGroups.Items.Add("Grains");
            cmbboxFGroups.Items.Add("Proteins");
            cmbboxFGroups.Items.Add("Diary");
            cmbboxFGroups.Items.Add("Oils & Solid Fats");
            cmbboxFGroups.Items.Add("Added Sugars");
            cmbboxFGroups.Items.Add("Bevarages");
            cmbboxFGroups.Items.Add("Others");

            currentRecipe = new Recipe();
        }

        /// <summary>
        /// Handles the click event of the Add Recipe button. 
        /// Retrieves values from the text boxes and combo box, creates a new Recipe object 
        /// with the entered values, adds the new recipe to the list box, and clears the 
        /// input fields (except for the recipe name text box).
        /// </summary>
        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRecipeName.Text) ||
                string.IsNullOrWhiteSpace(txtIngredientName.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtUnitMeasure.Text) ||
                string.IsNullOrWhiteSpace(txtCalories.Text) ||
                cmbboxFGroups.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields before adding an ingredient.", "Empty Fields", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) ||
                !int.TryParse(txtCalories.Text, out int calories))
            {
                MessageBox.Show("Please enter valid numbers for Quantity and Calories.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string recipeName = txtRecipeName.Text;
            string ingredientName = txtIngredientName.Text;
            string unitOfMeasure = txtUnitMeasure.Text;
            string foodGroup = cmbboxFGroups.SelectedItem.ToString();

            Ingredient ingredient = new Ingredient
            {
                Name = ingredientName,
                Quantity = quantity,
                UnitOfMeasure = unitOfMeasure,
                Calories = calories,
                FoodGroup = foodGroup
            };

            currentRecipe.Name = recipeName;
            currentRecipe.Ingredients.Add(ingredient);

            txtIngredientName.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtUnitMeasure.Text = string.Empty;
            txtCalories.Text = string.Empty;
            cmbboxFGroups.SelectedIndex = -1;

            UpdateListBox();
            MessageBox.Show("Ingredient added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the click event of the Clear Boxes button.
        /// Clears the text boxes and resets the combo box to its default state.
        /// </summary>        
        private void btnClearBoxs_Click(object sender, RoutedEventArgs e)
        {
            isClearing = true;
            // Clear all fields including recipe name
            txtRecipeName.Text = string.Empty;
            txtIngredientName.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtUnitMeasure.Text = string.Empty;
            txtCalories.Text = string.Empty;
            txtSteps.Text = string.Empty;
            cmbboxFGroups.SelectedIndex = -1;

            // Reset the current recipe
            currentRecipe = new Recipe();

            isClearing = false;
        }

        /// <summary>
        /// Handles the PreviewTextInput event for a text box to allow only integer input.
        /// Prevents non-digit characters from being entered in the text box.
        /// </summary>        
        private void IntOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        /// <summary>
        /// Checks if the input text contains only digits.
        /// </summary>        
        private static bool IsTextAllowed(string text)
        {
            // Regular expression to match only digits
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        /// <summary>
        /// Handles the click event of the Confirm Recipes button.
        /// Shows the main window and closes the current window.
        /// </summary>
        private void btnConfirmRecipes_Click(object sender, RoutedEventArgs e)
        {
            if (currentRecipe != null && !string.IsNullOrWhiteSpace(currentRecipe.Name))
            {
                if (MainWindow.AllRecipes.Any(r => r.Name.Equals(currentRecipe.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A recipe with this name already exists. Please choose a different name.", "Duplicate Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MainWindow.AllRecipes.Add(currentRecipe);
                MessageBox.Show("Recipe added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please add a recipe before confirming.", "No Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        /// <summary>
        /// Handles the click event of the Add Steps button.
        /// Retrieves steps from the text box, adds them to the current recipe, and updates the UI.
        /// Displays a message if the steps text box is empty.
        /// </summary>        
        private void btnAddSteps_Click(object sender, RoutedEventArgs e)
        {
            string steps = txtSteps.Text.Trim();
            if (!string.IsNullOrWhiteSpace(steps))
            {
                currentRecipe.AddStep(steps);
                txtSteps.Text = string.Empty;
                UpdateListBox();
            }
            else
            {
                MessageBox.Show("Please enter the steps before adding them to the recipe.", "Empty Steps", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Updates the list box with the current recipe.
        /// Checks if the list box already contains the current recipe. If it does, updates it; otherwise, adds it.
        /// </summary>        
        private void UpdateListBox()
        {
            lstBoxRecipe.Items.Clear();
            if (string.IsNullOrWhiteSpace(currentRecipe.Name) && currentRecipe.Ingredients.Count == 0)
            {
                lstBoxRecipe.Items.Add("Recipe Name: ");
                lstBoxRecipe.Items.Add("No ingredient added yet.");
            }
            else
            {
                lstBoxRecipe.Items.Add(currentRecipe.ToString());
            }
        }

        private void txtRecipeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isClearing)
            {
                currentRecipe.Name = txtRecipeName.Text;
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
