using System;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    [SerializeField] private string id = string.Empty;

    private void Start()
    {
        if (id == string.Empty) Debug.LogError($"{gameObject.name} SaveableEntity id not set up");
    }

    public string ID => id;

    [ContextMenu("Generate GUID")]
    private void GenerateGUID() => id = Guid.NewGuid().ToString();

    public SerializableDictionary<string, SerializableList> SaveData()
    {
        SerializableDictionary<string, SerializableList> data = new SerializableDictionary<string, SerializableList>();

        foreach(var saveable in GetComponents<iSaveable>())
        {
            data[saveable.GetType().ToString()] = saveable.SaveData();
            Debug.Log(saveable.SaveData());
        }

        return data;
    }

    public void LoadData(SerializableDictionary<string, SerializableList> _data)
    {
        SerializableDictionary<string, SerializableList> data = (SerializableDictionary<string, SerializableList>)_data;

        foreach(var saveable in GetComponents<iSaveable>())
        {
            string typeName = saveable.GetType().ToString();

            if(data.TryGetValue(typeName, out SerializableList value))
            {
                saveable.LoadData(value);
            }
        }
    }
}
