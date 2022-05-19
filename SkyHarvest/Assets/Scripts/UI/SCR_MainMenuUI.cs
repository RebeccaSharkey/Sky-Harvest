using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_MainMenuUI : MonoBehaviour
{
    [SerializeField] private string playScene;

    [Header("Sub Menus")]
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject newGameMenu;
    [SerializeField] private GameObject loadMenu;

    [Header("Load Scene stuff")]
    [SerializeField] private List<string> gameScenes;
    [SerializeField] private GameObject loadMenuContainer;
    [SerializeField] private GameObject loadButton;


    public void Start()
    {
        //if(gameScenes.Count > 0)
        //{
        //    foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        //    {
        //        string s = scene.path.ToString().Substring(scene.path.ToString().LastIndexOf("/"));
        //        s = s.Remove(0, 1);
        //        s = s.Replace(".unity", "");
        //        if (s != "Main Menu" && s != "Loading" && s != "GameScene")
        //        {
        //            GameObject newButton = Instantiate(loadButton, loadMenuContainer.transform);
        //            newButton.GetComponentInChildren<TextMeshProUGUI>().text = s;
        //            newButton.GetComponent<Button>().onClick.AddListener(delegate { OnLoadOption(s); });
        //        }
        //    }
        //}

        for(int i = 0; i < 3; i++)
        {
            loadMenuContainer.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = SaveLoadManager.instance.CheckSaveExists(i);
        }
    }

    public void OnStartNewOption(int _file)
    {
        SaveLoadManager.instance.PickFile(_file);
        SaveLoadManager.instance.DeleteFile();
        LoadNewGameScene();
    }

    public void OnLoadOption(int _file)
    {
        SaveLoadManager.instance.PickFile(_file);
        LoadGameScene();
    }

    private void LoadNewGameScene()
    {
        CustomEvents.SceneManagement.OnLoadNewScene?.Invoke("IntroCut");
    }

    private void LoadGameScene()
    {
        CustomEvents.SceneManagement.OnLoadNewScene?.Invoke("Farm");
    }

    public void OnNewGameButton()
    {
        newGameMenu.SetActive(true);
    }

    public void OnLoadButton()
    {
        loadMenu.SetActive(true);
    }

    public void OnOptionMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnCloseMenuButton()
    {
        newGameMenu.SetActive(false);
        loadMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
