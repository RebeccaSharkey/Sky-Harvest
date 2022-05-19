using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableList : List<string>, ISerializationCallbackReceiver
{
    [SerializeField] private string data;

    public void OnBeforeSerialize()
    {
        //data = string.Empty;

        //foreach (string value in this)
        //{
        //    data += value.ToString() + ',';
        //}

        data = string.Join(",", this);
        
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        string[] values = data.Split(',');
        foreach (string value in values)
        {
            this.Add(value);
        }
    }
}
