#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using ScenarioManagement;

[CreateAssetMenu(fileName ="new ScenarioInfo", menuName="ScriptableObjects/Scenario Info")]
public class ScenarioInfo : ScriptableObject
{
    private const string IDENTIFIER_SEPARATOR = "_"; //DONT CHANGE

    [Header("Scenario Info")]
    [SerializeField] private string _scenarioName;
    [SerializeField] private string _identifier;
    [SerializeField] [TextArea(5, 10)] private string _description;
    [SerializeField] [Range(0,10)] private int _difficulty;
    [Space]
    [SerializeField] ScoreMode _scoreMode = ScoreMode.HigherIsBetter;
    [Header("Presets")]
    [SerializeField] private int[] _timePresets = new int[] { 15,30,60,120};
    [SerializeField] private int _defaultTimePresetIndex = 2;
    [SerializeField] private int[] _amountToKillPresets = new int[] { 10,25,50,100};
    [SerializeField] private int _defaultAmountToKillPreset = 2;
    [Header("Map Info")]
    [Space]
    [SerializeField] private SceneReference _scene;
    [SerializeField] private GameObject _map;
    [SerializeField] private Vector3 _mapPos;

    private void OnEnable() {
        scenePath = _scene.ScenePath;
#if UNITY_EDITOR
        if(_defaultTimePresetIndex < 0 || _defaultTimePresetIndex >= _timePresets.Length) {
            Debug.LogError("Default Time Preset Index in " + _scenarioName + "scriptable object is invalid!");
        }

        if (_defaultAmountToKillPreset < 0 || _defaultAmountToKillPreset >= _amountToKillPresets.Length) {
            Debug.LogError("Default Amount to Kill Preset Index in " + _scenarioName + "scriptable object is invalid!");
        }
#endif
    }

    public string scenarioName => _scenarioName;
    public string identifier => _identifier;
    public string description => _description;
    public int difficulty => _difficulty;

    public ScoreMode scoreMode => _scoreMode;

    public int[] timePresets => _timePresets;
    public int defaultTimePresetIndex => _defaultTimePresetIndex;
    public int[] amountToKillPresets => _amountToKillPresets;
    public int defaultAmountToKillPreset => _defaultAmountToKillPreset;

    public SceneReference scene => _scene;
    public string scenePath { get; private set; }
    public GameObject map => _map;
    public Vector3 mapPos => _mapPos;

    public string GenerateTimedIdentifier(int timeAllowed) {
        return string.Join(IDENTIFIER_SEPARATOR, identifier, "T" + timeAllowed.ToString());
    }

    public string GenerateTimeTrialIdentifier(int amountToKill) {
        return string.Join(IDENTIFIER_SEPARATOR, identifier, "TOX" + amountToKill.ToString());
    }
}
