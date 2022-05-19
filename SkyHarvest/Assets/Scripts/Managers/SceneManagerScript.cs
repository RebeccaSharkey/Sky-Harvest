using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEditor;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript sceneManager;

    private bool isNewGame = false;

    public void Awake()
    {
        if (sceneManager != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            sceneManager = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void ChangeScene(string newScene)
    {
        StartCoroutine(LoadingScene(newScene));


        //Only used in unbuild versions of the game as a check to see if the scenes are infact in the game 
        /*string checkScene = "/" + newScene + ".unity";
        bool found = false;
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string s = scene.path.ToString().Substring(scene.path.ToString().LastIndexOf("/"));
            if (s == checkScene)
            {
                found = true;
                break;
            }
        }
        if(found)
        {
            StartCoroutine(LoadingScene(newScene));
        }
        else
        {
            Debug.LogWarning(string.Format("{0} is not in the build settings please add to open", newScene));
        }*/
    }

    private IEnumerator LoadingScene(string scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading");
        yield return new WaitForSeconds(1f);
        AsyncOperation asycOp = SceneManager.LoadSceneAsync(scene);
        while (!asycOp.isDone)
        {
            yield return null;
        }

        if(isNewGame)
        {
            yield return new WaitForSeconds(1f);
            CustomEvents.Tutorial.OnStartTutorial?.Invoke();
        }
        yield return null;
    }

    private void SetIsNewGame(bool newBool)
    {
        isNewGame = newBool;
    }

    private void OnEnable()
    {
        CustomEvents.SceneManagement.OnLoadNewScene += ChangeScene;
        CustomEvents.SceneManagement.OnSetNewGame += SetIsNewGame;
    }

    private void OnDisable()
    {
        CustomEvents.SceneManagement.OnLoadNewScene -= ChangeScene;
        CustomEvents.SceneManagement.OnSetNewGame -= SetIsNewGame;
    }
}
