using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMapper : MonoBehaviour
{

    protected virtual void Awake() {
        GetCaches();
        NagaUnityUtilities.NagaUtils.ExecuteAfterOneFrame(() => {
            MapFromSettings();
            ApplySettings();
            SetupInputEvents();
        });
    }

    protected virtual void Start() {

    }

    protected virtual void GetCaches() {

    }

    protected virtual void SetupInputEvents() {

    }

    public virtual void ApplySettings() {

    }
    //^^ Sometimes settings have to be explicitly announced when changed - such as resolution or vsync. Sometimes, it doesn't matter, such as sensitivity

    public virtual void MapToSettings() {

    }

    public virtual void MapFromSettings() {

    }

    //Maybe abstract it so when others call, applysettings always called and what is override is a method with the same name but with _ at the front? TODO
}
