using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName ="ScriptableObjects/Singletons/KeySceneReferences", fileName ="new KeySceneReferences")]
public class KeySceneReferences : ScriptableObject, ICanBeSingleton
{
    [SerializeField] private SceneReference menuScreen;

    private string _menuPath;

    public void OnEnable() {
        _menuPath = menuScreen.ScenePath;
    }

    public string menuPath => _menuPath;
}
