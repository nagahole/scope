using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMapperContainer : MonoBehaviour
{
    [SerializeField] private SettingsMapper[] mappers;

    public void MapAllFromSettings() {
        for(int i = 0; i < mappers.Length; i++) {
            mappers[i].MapFromSettings();
        }
    }

    public void MapAllToSettings() {
        for (int i = 0; i < mappers.Length; i++) {
            mappers[i].MapToSettings();
        }
        Settings.instance.Save();
    }
}
