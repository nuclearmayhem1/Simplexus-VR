using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour
{
    public LevelSettings settings;
    private string path = "Assets/Project/Levels/";
    public TMP_InputField levelNameField;
    public Slider diameterSlider;
    public Level currentLevel = null;
    private bool lockSettings = true;
    public Transform levelOrigin;
    public GameObject newLevelPrefab;
    public TMP_Text newLevelText;
    public Transform scaleOffset;

    public void OnSettingsChanged()
    {
        if (!lockSettings)
        {
            settings.name = levelNameField.text;
            settings.diameter = diameterSlider.value;
            currentLevel.Terrain.transform.localScale = Vector3.one * settings.diameter;
            scaleOffset.localPosition = new Vector3(0, 0, 0.5f) * diameterSlider.value;
        }
    }

    public void NewLevel()
    {
        if (currentLevel == null)
        {
            Debug.Log("New level");
            GameObject temp = Instantiate(newLevelPrefab, levelOrigin);
            currentLevel = temp.GetComponent<Level>();
            lockSettings = false;
            newLevelText.text = "Lock settings";
        }
        else
        {
            if (!lockSettings)
            {
                lockSettings = true;
                newLevelText.text = "Clear level";
                diameterSlider.interactable = false;
                levelNameField.interactable = false;
            }
            else
            {
                Destroy(currentLevel.gameObject);
                currentLevel = null;
                newLevelText.text = "New level";
                diameterSlider.interactable = true;
                levelNameField.interactable = true;
            }
        }
    }

    public void SaveLevel()
    {
        Debug.Log("Save level");
        //File.Create(path + settings.name);
    }

    public void LoadLevel()
    {
        lockSettings = true;
        Debug.Log("Load level");
        diameterSlider.interactable = false;
        levelNameField.interactable = false;
    }

}

[System.Serializable]
public struct LevelSettings
{
    public float diameter;
    public string name;
}