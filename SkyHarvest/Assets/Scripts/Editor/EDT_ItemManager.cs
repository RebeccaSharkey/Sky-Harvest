using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
public class EDT_ItemManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (ItemManager)target;

        if (GUILayout.Button("Refresh Lists", GUILayout.Height(40)))
        {
            script.Refresh();
        }
    }
}
