using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_RecipeButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Recipes recipe;

    public void SetUpButton(Recipes newRecipe)
    {
        recipe = newRecipe;
        text.text = recipe.GetProduct().name + " Recipe";
    }

    public void OnClick()
    {
        CustomEvents.CraftMachine.OnRecipeButtonPress?.Invoke(recipe);
    }

}
