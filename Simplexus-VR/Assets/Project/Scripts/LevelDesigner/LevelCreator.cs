using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelCreator : MonoBehaviour
{
    public LevelSettings settings;
    private string path = "Assets/Project/Levels/";

    public void OnSettingsChanged()
    {
        
    }

    public void NewLevel()
    {
        Debug.Log("New map");
    }

    public void SaveLevel()
    {
        Debug.Log("Save map");
        File.Create(path + settings.name);
    }

    public void LoadLevel()
    {
        Debug.Log("Load map");
    }

}

[System.Serializable]
public struct LevelSettings
{
    public float diameter;
    public string name;
}