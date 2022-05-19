using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EndCutScene : MonoBehaviour
{
    public void Close()
    {
        CustomEvents.SceneManagement.OnLoadNewScene?.Invoke("Farm");
    }
}
