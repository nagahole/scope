using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="new SensitivityPreset", menuName ="ScriptableObjects/SensitivityPreset")]
[System.Serializable]
public class SensitivityPreset : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private float _sensitivityMultiplier;
    [SerializeField] private Vector2 _range;

    public string displayName => _displayName;
    public float sensitivityMultiplier => _sensitivityMultiplier;
    public Vector2 range => _range;
}
