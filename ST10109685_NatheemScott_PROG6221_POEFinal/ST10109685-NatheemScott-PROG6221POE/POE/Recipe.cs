using POE;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using System.Windows;

namespace POE
{
    /// <summary>
    /// Represents a recipe, including the name, ingredient details, and preparation steps.
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Gets or sets the name of the recipe.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ingredient details for the recipe.
        /// </summary>
        public List<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Gets or sets the preparation steps for the recipe.
        /// </summary>
        public List<string> Steps { get; set; }

        public bool IsSelected { get; set; }
        public bool IsScaled { get; private set; }
        private List<Ingredient> OriginalIngredients { get; set; }

        /// <summary>
        /// Constructor to initialize a new Recipe object.
        /// </summary>
        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
        }

        /// <summary>
        /// Adds a step to the recipe's list of preparation steps.
        /// </summary>
        /// <param name="step">The step to add.</param>
        public void AddStep(string step)
        {
            Steps.Add(step);
        }

        /// <summary>
        /// Scales the recipe's ingredients and updates their quantities and calories.
        /// </summary>
        /// <param name="scalingFactor">The factor by which to scale the recipe (0.5 for half, 2 for double, etc.).</param>
        public void ScaleRecipe(double scalingFactor)
        {
            if (!IsScaled)
            {
                OriginalIngredients = new List<Ingredient>(Ingredients.Select(i => new Ingredient
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    UnitOfMeasure = i.UnitOfMeasure,
                    Calories = i.Calories,
                    FoodGroup = i.FoodGroup
                }));
            }

            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity = Math.Round(ingredient.Quantity * scalingFactor, 1);
                ingredient.Calories = Math.Round(ingredient.Calories * scalingFactor, 1);
            }

            IsScaled = true;
        }


        public void ResetToOriginal()
        {
            if (IsScaled && OriginalIngredients != null)
            {
                Ingredients = new List<Ingredient>(OriginalIngredients);
                IsScaled = false;
            }
            else
            {
                MessageBox.Show("No scaled recipes found. Nothing to reset.");
            }
        }


        /// <summary>
        /// Overrides the default ToString method to provide a formatted string representation of the Recipe object.
        /// Includes the recipe name, ingredient details (if available), and steps.
        /// </summary>
        /// <returns>A string representation of the Recipe object.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Recipe Name: {Name}");

            // Display Ingredients
            if (Ingredients != null && Ingredients.Count > 0)
            {
                sb.AppendLine("Ingredients:");
                foreach (var ingredient in Ingredients)
                {
                    sb.AppendLine($"- {ingredient.Quantity} {ingredient.UnitOfMeasure} of {ingredient.Name}, Has {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}");
                }
            }

            // Display Steps
            if (Steps != null && Steps.Count > 0)
            {
                sb.AppendLine("Steps:");
                for (int i = 0; i < Steps.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {Steps[i]}");
                }
            }

            return sb.ToString();
        }

    }

    /// <summary>
    /// Represents an ingredient, including the name, quantity, unit of measurement, calories, and food group.
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// Gets or sets the name of the ingredient.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the ingredient.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement for the ingredient.
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Gets or sets the calorie count of the ingredient.
        /// </summary>
        public double Calories { get; set; }

        /// <summary>
        /// Gets or sets the food group of the ingredient.
        /// </summary>
        public string FoodGroup { get; set; }
    }
}