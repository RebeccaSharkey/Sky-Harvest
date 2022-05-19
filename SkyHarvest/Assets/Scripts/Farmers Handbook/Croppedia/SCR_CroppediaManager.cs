using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_CroppediaManager : MonoBehaviour
{
    [Header("Initial Data")]
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private TextMeshProUGUI leftPageNumber;
    [SerializeField] private TextMeshProUGUI rightPageNumber;
    private int currentPage;
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backwardsButton;

    [Header("Page One")]
    [SerializeField] private Image pageOneIcon;
    [SerializeField] private TextMeshProUGUI pageOneTitle;
    [SerializeField] private TextMeshProUGUI rewardsTextOne;
    [SerializeField] private TextMeshProUGUI badSellOne;
    [SerializeField] private TextMeshProUGUI normalSellOne;
    [SerializeField] private TextMeshProUGUI goodSellOne;
    [SerializeField] private TextMeshProUGUI perfectSellOne;
    [SerializeField] private TextMeshProUGUI loreOne;
    [SerializeField] private GameObject pageOneModelToView;
    [SerializeField] private Button pageOneInspectButton;

    [Header("Page Two")]
    [SerializeField] private Image pageTwoIcon;
    [SerializeField] private GameObject pageTwoText;
    [SerializeField] private TextMeshProUGUI pageTwoTitle;
    [SerializeField] private TextMeshProUGUI rewardsTextTwo;
    [SerializeField] private TextMeshProUGUI badSellTwo;
    [SerializeField] private TextMeshProUGUI normalSellTwo;
    [SerializeField] private TextMeshProUGUI goodSellTwo;
    [SerializeField] private TextMeshProUGUI perfectSellTwo;
    [SerializeField] private TextMeshProUGUI loreTwo;
    [SerializeField] private GameObject pageTwoModelToView;
    [SerializeField] private Button pageTwoInspectButton;

    public void Open()
    {
        currentPage = 0;
        OnForward(); 
    }

    private void OpenFromButton(SCR_CropItems currentCrop)
    {
        CustomEvents.FarmersHandbook.OnOpenCropPediaPage?.Invoke();
        currentPage = 0;
        bool itemFound = false;
        while(!itemFound)
        {
            OnForward();
            if (itemManager.allCropItems.Count >= currentPage)
            {
                if(itemManager.allCropItems[currentPage - 2] == currentCrop)
                {
                    itemFound = true;
                }
                else if(itemManager.allCropItems[currentPage - 1] == currentCrop)
                {
                    itemFound = true;
                }
            }
            else if (itemManager.allCropItems.Count == currentPage - 1)
            {
                if (itemManager.allCropItems[currentPage - 2] == currentCrop)
                {
                    itemFound = true;
                }
            }
        }
    }

    public void OnForward()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Flip Page");
        currentPage += 2;
        TurnPage();
        if(currentPage >= itemManager.allCropItems.Count)
        {
            forwardButton.SetActive(false);
        }
        else
        {
            forwardButton.SetActive(true);
        }

        if(currentPage == 2)
        {
            backwardsButton.SetActive(false);
        }
        else
        {
            backwardsButton.SetActive(true);
        }
    }

    public void OnBackwards()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Flip Page");
        currentPage -= 2;
        TurnPage();
        if (currentPage >= itemManager.allCropItems.Count)
        {
            forwardButton.SetActive(false);
        }
        else
        {
            forwardButton.SetActive(true);
        }

        if (currentPage == 2)
        {
            backwardsButton.SetActive(false);
        }
        else
        {
            backwardsButton.SetActive(true);
        }
    }

    private void TurnPage()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Flip Page");
        if (itemManager.allCropItems.Count >= currentPage)
        {
            SetUpPage(1, itemManager.allCropItems[currentPage - 2]);
            SetUpPage(2, itemManager.allCropItems[currentPage - 1]);
            pageTwoIcon.gameObject.SetActive(true);
            pageTwoTitle.gameObject.SetActive(true);
            pageTwoText.gameObject.SetActive(true);
            pageTwoInspectButton.gameObject.SetActive(true);
        }
        else if (itemManager.allCropItems.Count == currentPage - 1)
        {
            SetUpPage(1, itemManager.allCropItems[currentPage - 2]);
            pageTwoIcon.gameObject.SetActive(false);
            pageTwoTitle.gameObject.SetActive(false);
            pageTwoText.gameObject.SetActive(false);
            pageTwoInspectButton.gameObject.SetActive(false);
        }
        leftPageNumber.text = (currentPage - 1).ToString();
        rightPageNumber.text = currentPage.ToString();
    }

    private void SetUpPage(int page, SCR_CropItems currentItem)
    {
        switch (page)
        {
            case 1:
                pageOneModelToView = currentItem.ItemModel;
                pageOneIcon.sprite = currentItem.ItemSprite;
                pageOneTitle.text = currentItem.ItemName;
                pageOneTitle.text = pageOneTitle.text.Replace("Tree", "");
                CustomEvents.ModelViewer.OnChangeModel?.Invoke(pageOneModelToView);
                GameObject tempOne = (GameObject)Resources.Load($"Prefabs/Crops/{currentItem.ItemName}");
                if (tempOne != null)
                {
                    CropBehaviour cropBehaviour = tempOne.GetComponent<CropBehaviour>();
                    rewardsTextOne.text = string.Format("~ {0} - {1} Crop(s)\n~ {2} Seeds", cropBehaviour.GetCropData.baseMinHarvest, cropBehaviour.GetCropData.baseMaxHarvest, cropBehaviour.GetCropData.baseMinSeedsReturned);
                    badSellOne.text = currentItem.SellValueBad.ToString();
                    normalSellOne.text = currentItem.SellValueNormal.ToString();
                    goodSellOne.text = currentItem.SellValueGood.ToString();
                    perfectSellOne.text = currentItem.SellValuePerfect.ToString();
                    loreOne.text = currentItem.Lore;
                }
                else if (currentItem.name == "Dead Crop")
                {
                    rewardsTextOne.text = "Dead Crop Received After Plant Dies.";
                    badSellOne.text = currentItem.SellValueBad.ToString();
                    normalSellOne.text = currentItem.SellValueNormal.ToString();
                    goodSellOne.text = currentItem.SellValueGood.ToString();
                    perfectSellOne.text = currentItem.SellValuePerfect.ToString();
                    loreOne.text = currentItem.Lore;
                }
                else
                {
                    tempOne = (GameObject)Resources.Load($"Prefabs/Crops/Fruit Trees/{currentItem.ItemName}");
                    if (tempOne != null)
                    {
                        FruitBehaviour cropBehaviour = tempOne.GetComponent<FruitBehaviour>();
                        rewardsTextOne.text = "";
                        foreach (SCR_Items item in cropBehaviour.GetFruitTreeSO.harvestables)
                        {
                            rewardsTextOne.text = string.Format("~ {0} {1}(s)", cropBehaviour.GetFruitTreeSO._minAmountOfProduce, item.name);
                        }
                        badSellOne.text = currentItem.SellValueBad.ToString();
                        normalSellOne.text = currentItem.SellValueNormal.ToString();
                        goodSellOne.text = currentItem.SellValueGood.ToString();
                        perfectSellOne.text = currentItem.SellValuePerfect.ToString();
                        loreOne.text = currentItem.Lore;
                    }
                }
                break;
            case 2:
                pageTwoModelToView = currentItem.ItemModel;
                pageTwoIcon.sprite = currentItem.ItemSprite;
                pageTwoTitle.text = currentItem.ItemName;
                pageTwoTitle.text = pageTwoTitle.text.Replace("Tree", "");
                CustomEvents.ModelViewer.OnChangeModel?.Invoke(pageOneModelToView);
                GameObject tempTwo = (GameObject)Resources.Load($"Prefabs/Crops/{currentItem.ItemName}");
                if (tempTwo != null)
                {
                    CropBehaviour cropBehaviour = tempTwo.GetComponent<CropBehaviour>();
                    rewardsTextTwo.text = string.Format("~ {0} - {1} Crop(s)\n~ {2} Seeds", cropBehaviour.GetCropData.baseMinHarvest, cropBehaviour.GetCropData.baseMaxHarvest, cropBehaviour.GetCropData.baseMinSeedsReturned);
                    badSellTwo.text = currentItem.SellValueBad.ToString();
                    normalSellTwo.text = currentItem.SellValueNormal.ToString();
                    goodSellTwo.text = currentItem.SellValueGood.ToString();
                    perfectSellTwo.text = currentItem.SellValuePerfect.ToString();
                    loreTwo.text = currentItem.Lore;
                }
                else if (currentItem.name == "Dead Crop")
                {
                    rewardsTextTwo.text = "Dead Crop Received After Plant Dies.";
                    badSellTwo.text = currentItem.SellValueBad.ToString();
                    normalSellTwo.text = currentItem.SellValueNormal.ToString();
                    goodSellTwo.text = currentItem.SellValueGood.ToString();
                    perfectSellTwo.text = currentItem.SellValuePerfect.ToString();
                    loreTwo.text = currentItem.Lore;
                }
                else
                {
                    tempTwo = (GameObject)Resources.Load($"Prefabs/Crops/Fruit Trees/{currentItem.ItemName}");
                    if (tempTwo != null)
                    {
                        FruitBehaviour cropBehaviour = tempTwo.GetComponent<FruitBehaviour>();
                        rewardsTextTwo.text = "";
                        foreach (SCR_Items item in cropBehaviour.GetFruitTreeSO.harvestables)
                        {
                            rewardsTextTwo.text = string.Format("~ {0} {1}(s)", cropBehaviour.GetFruitTreeSO._minAmountOfProduce, item.name);
                        }
                        badSellTwo.text = currentItem.SellValueBad.ToString();
                        normalSellTwo.text = currentItem.SellValueNormal.ToString();
                        goodSellTwo.text = currentItem.SellValueGood.ToString();
                        perfectSellTwo.text = currentItem.SellValuePerfect.ToString();
                        loreTwo.text = currentItem.Lore;
                    }
                }
                break;
        }
    }

    public void OpenModelToView(int page)
    {
        switch (page)
        {
            case 1:
                CustomEvents.ModelViewer.OnChangeModel?.Invoke(pageOneModelToView);
                break;
            case 2:
                CustomEvents.ModelViewer.OnChangeModel?.Invoke(pageTwoModelToView);
                break;
        }
    }

    private void OnEnable()
    {
        CustomEvents.FarmersHandbook.OnOpenCroppedia += OpenFromButton;
    }
    private void OnDisable()
    {
        CustomEvents.FarmersHandbook.OnOpenCroppedia -= OpenFromButton;
    }
}
