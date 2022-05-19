using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_IntroCutScene : MonoBehaviour
{
    public void EndCutScene()
    {
        CustomEvents.SceneManagement.OnSetNewGame?.Invoke(true);
        CustomEvents.SceneManagement.OnLoadNewScene?.Invoke("Farm");
    }
}
