using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Using a scriptablesingleton for  this is too complicated because this will need its own canvas
/// </summary>
public class ScenarioSelectionOptions : MonoBehaviour
{
    private static readonly Dictionary<ScenarioPlayMode, string> map = new Dictionary<ScenarioPlayMode, string>() {
        {ScenarioPlayMode.Timed, "Timed" },
        {ScenarioPlayMode.TimeTrial, "Time Trial" },
        {ScenarioPlayMode.Untimed, "Untimed" }

    };

    private static readonly ScenarioPlayMode[] playmodes = new ScenarioPlayMode[] {
        ScenarioPlayMode.Timed,
        ScenarioPlayMode.TimeTrial,
        ScenarioPlayMode.Untimed
    };

    public static ScenarioSelectionOptions instance { get; private set; }

    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform rect;
    [Space]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI highscoreDescriber;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [Space]
    [SerializeField] private TMP_Dropdown modeDropdown;
    [SerializeField] private TMP_Dropdown timeDropdown;
    [SerializeField] private TMP_Dropdown amountToKillDropdown;
    [Space]
    [SerializeField] private GameObject mainContainer;
    [SerializeField] private GameObject timedContainer, untimedContainer, timetrialContainer;
    [Space]
    [SerializeField] private UnityEvent onPositionChange;

    public ScenarioInfo scenarioInfo { get; private set; }
    public bool shown { get; private set; }

    private bool cursorIn;
    

    private void Awake() {
        instance = this;
        
        cursorIn = false;

        SetupModeDropdown();
        modeDropdown.onValueChanged.AddListener(OnModeChanged);
        modeDropdown.onValueChanged.AddListener(FetchHighscore);
        timeDropdown.onValueChanged.AddListener(FetchHighscore);
        amountToKillDropdown.onValueChanged.AddListener(FetchHighscore);

        Hide();
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0) && !cursorIn) {
            Hide();
        }
    }

    private void FetchHighscore(int x = 0) { //added int just to get get it to work with UnityEvent<int>
        var mode = playmodes[modeDropdown.value];
        if (mode == ScenarioPlayMode.Timed) {
            highscoreDescriber.text = "Highscore";
            int timeAllowed = scenarioInfo.timePresets[timeDropdown.value];

            var result = SOSingleton<ScenarioManager>.sharedInstance.highscoreTracker.
                TryGetHighscore(scenarioInfo.GenerateTimedIdentifier(timeAllowed));

            if (!result.exists) {
                highscoreText.text = "-/-";
            } else {
                highscoreText.text = result.score.ToString("0"); //1dp with 0 if nothing
            }

        } else if(mode == ScenarioPlayMode.TimeTrial) {
            highscoreDescriber.text = "Best Time";
            int amountToKill = scenarioInfo.amountToKillPresets[amountToKillDropdown.value];

            var result = SOSingleton<ScenarioManager>.sharedInstance.highscoreTracker.
                TryGetHighscore(scenarioInfo.GenerateTimeTrialIdentifier(amountToKill));

            if (!result.exists) {
                highscoreText.text = "-/-";
            } else {
                highscoreText.text = result.score.ToMinutesAndSecondsFormatted(2);
            }

        } else if(mode == ScenarioPlayMode.Untimed) {
            highscoreDescriber.text = string.Empty;
            highscoreText.text = string.Empty;
        } else {
            Debug.LogError("Mode unrecognised in FetchHighscore!");
        }
    }

    private void SetupModeDropdown() {
        modeDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < playmodes.Length; i++) {
            options.Add(map[playmodes[i]]);
        }
        modeDropdown.AddOptions(options);
        modeDropdown.value = 0;
    }

    private void SetupTimedPresets() {
        timeDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < scenarioInfo.timePresets.Length; i++) {
            options.Add(scenarioInfo.timePresets[i].ToString() + "s");
        }
        timeDropdown.AddOptions(options);
        timeDropdown.value = scenarioInfo.defaultTimePresetIndex;
    }

    private void SetupAmountToKillPresets() {
        amountToKillDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < scenarioInfo.amountToKillPresets.Length; i++) {
            options.Add(scenarioInfo.amountToKillPresets[i].ToString() + " KILLS");
        }
        amountToKillDropdown.AddOptions(options);
        amountToKillDropdown.value = scenarioInfo.defaultAmountToKillPreset;
    }

    private void OnModeChanged(int value) {
        ScenarioPlayMode mode = playmodes[value]; //mapped from this, so can be reversed
        if (mode == ScenarioPlayMode.Timed) {
            timedContainer.gameObject.SetActive(true);
            timetrialContainer.gameObject.SetActive(false);
            untimedContainer.gameObject.SetActive(false);
        } else if (mode == ScenarioPlayMode.TimeTrial) {
            timedContainer.gameObject.SetActive(false);
            timetrialContainer.gameObject.SetActive(true);
            untimedContainer.gameObject.SetActive(false);
        } else if (mode == ScenarioPlayMode.Untimed) {
            timedContainer.gameObject.SetActive(false);
            timetrialContainer.gameObject.SetActive(false);
            untimedContainer.gameObject.SetActive(true);
        } else {
            Debug.LogError("Unknown playmode discovered. Index out of range or new mode not implemented?");
        }
    }

    public void SetScenarioInfo(ScenarioInfo scenarioInfo) {
        this.scenarioInfo = scenarioInfo;
        titleText.text = scenarioInfo.name;

        modeDropdown.value = 0;
        OnModeChanged(0);

        SetupAmountToKillPresets();
        SetupTimedPresets();
    }

    public void Play() {
        ScenarioPlayMode mode = playmodes[modeDropdown.value];
        if (mode == ScenarioPlayMode.Timed) {
            SOSingleton<ScenarioManager>.sharedInstance.PlayScenarioTimed(scenarioInfo, scenarioInfo.timePresets[timeDropdown.value]);
        } else if (mode == ScenarioPlayMode.TimeTrial) {
            SOSingleton<ScenarioManager>.sharedInstance.PlayScenarioTimeTrial(scenarioInfo, scenarioInfo.amountToKillPresets[amountToKillDropdown.value]);
        } else if (mode == ScenarioPlayMode.Untimed) {
            SOSingleton<ScenarioManager>.sharedInstance.PlayScenarioUntimed(scenarioInfo);
        } else {
            Debug.LogError("Unknown playmode discovered. Index out of range or new mode not implemented?");
        }
    }

    public void Show() {
        NagaUnityUtilities.NagaUtils.ExecuteAfterOneFrame(() => {
            shown = true;
            mainContainer.SetActive(true);
        });
    }

    public void TweenIn() {
        animator.SetTrigger("TweenIn");
    }

    public void Goto(Vector2 absoluteRectPosition) {
        rect.position = absoluteRectPosition;
        onPositionChange.Invoke();
    }

    public void GotoCentre() {
        rect.anchoredPosition = new Vector2(0, 0);
        onPositionChange.Invoke();
    }

    public void Hide() {
        shown = false;
        mainContainer.SetActive(false);
    }

    public void SetCursorIn() {
        cursorIn = true;
    }

    public void SetCursorOut() {
        cursorIn = false;
    }
}
