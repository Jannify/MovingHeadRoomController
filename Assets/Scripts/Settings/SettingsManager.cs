using System;
using System.IO;
using UnityEngine;

public struct SettingsHolder
{
    public string IP;
    public string SubMask;
    public bool SendData;
}

public class SettingsManager : MonoBehaviour
{
    private string path;

    public static SettingsHolder Settings;

    private void Awake()
    {
        path = Path.Combine(Application.persistentDataPath, "settings.json");

        LoadSettings();
    }

    private void LoadSettings()
    {
        if (!File.Exists(path))
        {
            Settings = new SettingsHolder();
            File.WriteAllText(path, JsonUtility.ToJson(Settings, true));
            Debug.Log("Saved settings at: " + path);
            return;
        }

        string text = File.ReadAllText(path);
        Settings = JsonUtility.FromJson<SettingsHolder>(text);
    }
}
