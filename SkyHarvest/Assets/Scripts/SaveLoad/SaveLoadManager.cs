using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SaveLoadManager : MonoBehaviour
{
    private string persistPath;
    public int selectedFile = 0;

    public static SaveLoadManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) Destroy(gameObject);

        persistPath = Application.persistentDataPath;
    }

    public bool CheckSaveExists(int _file)
    {
        return File.Exists(GetFullSaveFilePath(_file));
    }

    private string GetFullSaveFilePath(int _file)
    {
        return persistPath + "/saveData" + _file + ".scope";
    }

    public void PickFile(int _file) => selectedFile = _file;

    [ContextMenu("Save")]
    public void Save()
    {
        SerializableDictionary<string, SerializableDictionary<string, SerializableList>> data = LoadFile(selectedFile);
        CollectData(data);
        SaveFile(data);
        Debug.Log("Save finished");
    }

    [ContextMenu("Load")]
    public void Load()
    {
        SerializableDictionary<string, SerializableDictionary<string, SerializableList>> data = LoadFile(selectedFile);
        RestoreData(data);
        Debug.Log("Load finished");
    }

    public void DeleteFile()
    {
        if (CheckSaveExists(selectedFile))
        {
            File.Delete(GetFullSaveFilePath(selectedFile));
        }
    }

    private void SaveFile(SerializableDictionary<string, SerializableDictionary<string, SerializableList>> _data)
    {
        string jsonString = JsonUtility.ToJson(_data);

        Debug.Log(jsonString);

        File.WriteAllText(GetFullSaveFilePath(selectedFile), jsonString);
    }

    private SerializableDictionary<string, SerializableDictionary<string, SerializableList>> LoadFile(int _file)
    {
        if (!CheckSaveExists(_file))
        {
            //no save file
            return new SerializableDictionary<string, SerializableDictionary<string, SerializableList>>();
        }

        string jsonString = File.ReadAllText(GetFullSaveFilePath(_file));
        SerializableDictionary<string, SerializableDictionary<string, SerializableList>> data = JsonUtility.FromJson<SerializableDictionary<string, SerializableDictionary<string, SerializableList>>>(jsonString);

        return data;
    }

    private void CollectData(SerializableDictionary<string, SerializableDictionary<string, SerializableList>> _data)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            _data[saveable.ID] = saveable.SaveData();
        }
    }

    private void RestoreData(SerializableDictionary<string, SerializableDictionary<string, SerializableList>> _data)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(_data.TryGetValue(saveable.ID, out SerializableDictionary<string, SerializableList> value))
            {
                saveable.LoadData(value);
            }
        }
    }
}
