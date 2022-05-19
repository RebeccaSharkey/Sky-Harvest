using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Constructs the contextual menu from a list of menu elements and places it on the screen at the clicked position
/// </summary>
public class ContextualMenu
{
    private List<iContextualMenuable> menuElements;
    private Vector3 positionToCreate;

    public ContextualMenu(List<iContextualMenuable> _menuElements, Vector3 _clickPosition)
    {
        menuElements = _menuElements;
        positionToCreate = _clickPosition;

        CreateMenu();
    }

    public void CreateMenu()
    {
        CustomEvents.UI.OnDestroyContextualMenu?.Invoke();

        //Sets up the menu parent component
        GameObject menuParent = new GameObject("MenuParent", typeof(RectTransform), typeof(VerticalLayoutGroup));
        menuParent.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
        menuParent.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = false;
        menuParent.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
        menuParent.GetComponent<VerticalLayoutGroup>().childControlWidth = false;
        menuParent.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        menuParent.transform.position = positionToCreate;
        menuParent.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 100f);
        menuParent.AddComponent<ContextualMenuParentBehaviour>();

        //Adds each element from the list
        for(int i = 0; i < menuElements.Count; i++)
        {
            GameObject newMenuElement = Object.Instantiate(Resources.Load("Prefabs/ContextualMenuButton", typeof(GameObject))) as GameObject;
            newMenuElement.GetComponent<Button>().onClick.AddListener(menuElements[i].OnSelected);
            newMenuElement.transform.SetParent(menuParent.transform);
            newMenuElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = menuElements[i].GetButtonText();
        }
    }
}
