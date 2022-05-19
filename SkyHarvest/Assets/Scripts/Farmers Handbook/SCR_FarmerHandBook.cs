using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_FarmerHandBook : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;

    [SerializeField] private GameObject handbookUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject cropPediaUI;
    [SerializeField] private GameObject recipeBookUI;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject button;
 
    public void OpenCroppedia()
    {
        CustomEvents.TimeCycle.OnPause();
        handbookUI.SetActive(true);
        mainMenuUI.SetActive(false);
        recipeBookUI.SetActive(false);
        cropPediaUI.SetActive(true);
        button.SetActive(false); 
        foreach (GameObject objecty in objectsToHide)
        {
            objecty.SetActive(false);
        }
    }

    public void OpenRecipeBook()
    {
        CustomEvents.TimeCycle.OnPause();
        handbookUI.SetActive(true);
        mainMenuUI.SetActive(false);
        cropPediaUI.SetActive(false);
        recipeBookUI.SetActive(true);
        button.SetActive(false);
        foreach (GameObject objecty in objectsToHide)
        {
            objecty.SetActive(false);
        }
    }

    public void OnOpen()
    {
        CustomEvents.TimeCycle.OnPause();
        foreach (GameObject objecty in objectsToHide)
        {
            objecty.SetActive(false);
        }
    }

    public void OnClose()
    {
        CustomEvents.TimeCycle.OnUnpause();
        foreach (GameObject objecty in objectsToHide)
        {
            objecty.SetActive(true);
        }
        mainMenuUI.SetActive(true);
        tutorialUI.SetActive(false);
        cropPediaUI.SetActive(false);
        recipeBookUI.SetActive(false);
        handbookUI.SetActive(false);
    }

    private void OnEnable()
    {
        CustomEvents.FarmersHandbook.OnOpenCropPediaPage += OpenCroppedia;
        CustomEvents.FarmersHandbook.OnOpenRecipeBookPage += OpenRecipeBook;
    }

    private void OnDisable()
    {
        CustomEvents.FarmersHandbook.OnOpenCropPediaPage -= OpenCroppedia;
        CustomEvents.FarmersHandbook.OnOpenRecipeBookPage -= OpenRecipeBook;
    }
}
