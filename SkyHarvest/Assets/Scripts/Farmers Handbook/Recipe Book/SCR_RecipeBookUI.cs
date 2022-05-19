using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SCR_RecipeBookUI : MonoBehaviour
{
    [Header("Button Data")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonContainer;

    [Header("Right Side Data")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI mainText;

    public void OnOpen()
    {
        foreach (Transform item in buttonContainer.transform)
        {
            Destroy(item.gameObject);
        }

        List<Recipes> tempList = (List<Recipes>)CustomEvents.CraftMachine.OnGetListOfRecipes?.Invoke();
        if(tempList != null)
        {
            foreach(Recipes recipe in tempList)
            {
                GameObject newButton = Instantiate(buttonPrefab, transform.position, Quaternion.identity, gameObject.transform);
                newButton.GetComponent<SCR_RecipeButton>().SetUpButton(recipe);
                newButton.GetComponent<RectTransform>().SetParent(buttonContainer.gameObject.GetComponent<RectTransform>());
            }
        }

        title.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
    }

    private void UpdateRightSide(Recipes currentRecipe)
    {
        title.gameObject.SetActive(true);
        mainText.gameObject.SetActive(true);
        title.text = currentRecipe.GetProduct().name + " Recipe";
        KeyValuePair<SCR_Items, int> componentOne = currentRecipe.GetComponents()[0];
        KeyValuePair<SCR_Items, int> componentTwo = currentRecipe.GetComponents()[1];
        mainText.text = string.Format("{0}({1}) + {2}({3}) = {4}({5})", componentOne.Key.name, componentOne.Value, componentTwo.Key.name, componentTwo.Value, currentRecipe.GetProduct().name, currentRecipe.GetProductAmount());
    }

    private void OnEnable()
    {
        CustomEvents.CraftMachine.OnRecipeButtonPress += UpdateRightSide;
    }

    private void OnDisable()
    {
        CustomEvents.CraftMachine.OnRecipeButtonPress -= UpdateRightSide;
    }
}
