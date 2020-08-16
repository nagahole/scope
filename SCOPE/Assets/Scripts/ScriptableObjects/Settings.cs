using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new Settings", menuName = "ScriptableObjects/Singletons/Settings")]
public class Settings : ScriptableObject, ICanBeSingleton, IInitializable {
    [System.Serializable]
        private class SettingsData {
        public List<SettingsPreset> settingsPresets;
        public int currentSettingsIndex;

        public SettingsPreset selectedSettings {
            get {
                return settingsPresets[currentSettingsIndex];
            }
            set {
                settingsPresets[currentSettingsIndex] = value;
            }
        }

        public SettingsData(List<SettingsPreset> presets, int currentSettingsIndex){
            settingsPresets = presets;
            this.currentSettingsIndex = currentSettingsIndex;
        }
    }

    [Header("Default Values")]
    [SerializeField] private SettingsPreset defaultSettingPreset;
    [SerializeField] private int defaultSettingsIndex = 0;
    [SerializeField] private int defaultNumberOfPresets = 1;
    [Header("Runtime Values")]
    [SerializeField] private SettingsData settingsData;

    private const string RELATIVE_FILE_PATH = "/settings.gay";
    private string filePath;

    public static Settings instance => SOSingleton<Settings>.sharedInstance; //Bruh - Added this purely to type less when accessing settings
    public static SettingsPreset activeSettings {
        get{
            return instance.settingsData.selectedSettings;
        }
        set {
            instance.settingsData.selectedSettings = value;
        }
    }

    public void Initialize() {
        filePath = Application.persistentDataPath + RELATIVE_FILE_PATH;
        Debug.Log(filePath);
        Load();
    }

    private void DefaultValues() {
        Debug.Log("Defaulting Values");
        List<SettingsPreset> buffer = new List<SettingsPreset>();
        for(int i = 0; i < defaultNumberOfPresets; i++) {
            buffer.Add(defaultSettingPreset);
        }
        settingsData = new SettingsData(buffer, defaultSettingsIndex);
    }

    private void ClearSettings() {
        try {
            File.WriteAllText(filePath, string.Empty);
        } catch (System.Exception e) {
            Debug.LogError(e.Message);
        }
    }

    public void Save() {
        Debug.Log("SAVING SETTINGS");
        try {
            File.WriteAllText(filePath, JsonUtility.ToJson(settingsData, true));
        } catch (System.Exception e) {
            Debug.LogWarning(e.Message);
            DefaultValues();
        }
    }

    private void Load() {
        Debug.Log("LOADING SETTINGS");
        if (File.Exists(filePath)) {
            using(StreamReader stream = new StreamReader(filePath)) {
                string raw = stream.ReadToEnd();
                if(raw.Length == 0) {
                    DefaultValues();
                    return;
                }
                settingsData = JsonUtility.FromJson<SettingsData>(raw);
            }
            for(int i = settingsData.settingsPresets.Count; i < defaultNumberOfPresets; i++) {
                settingsData.settingsPresets.Add(defaultSettingPreset);
            }
        } else {
            DefaultValues();
        }
    }

    private void ApplyChanges() {
        Save();
    }
}