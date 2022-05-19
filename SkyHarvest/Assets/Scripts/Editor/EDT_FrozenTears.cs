using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FrozenTears))]
public class EDT_FrozenTears : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FrozenTears frozenTears = (FrozenTears)target;

        frozenTears.SetAmount(EditorGUILayout.IntField("Currency", frozenTears.amount));

        EditorUtility.SetDirty(frozenTears);
    }
}
