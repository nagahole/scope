using ScenarioManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NagaUnityUtilities;

/// <summary>
/// Returns the accuracy in decimals ie 0.844
/// </summary>
public class AccuracyChangeEvent : UnityEvent<float> { }

public class ScenarioSession
{
    private const string IDENTIFIER_SEPARATOR = "_"; //DONT CHANGE

    public ScenarioInfo scenarioInfo { get; private set; }
    public float score { get; private set; }
    public int hits { get; private set; }
    public int misses { get; private set; }
    public int kills { get; private set; }

    public ScenarioPlayMode playMode { get; private set; }
    public int timeAllowed { get; private set; }
    public int amountToKill { get; private set; }

    private IStatUI statUI;

    public float accuracy => (float) hits / (float) (hits + misses);

    private UnityEvent test;

    #region Events for unregistering
    private IScoreSystem scoreSystem;
    private IHitMissDeterminer hitMissDeterminer;
    private UnityEvent onKillEvent;
    private TimerTickedEvent timerTickedEvent;
    #endregion

    public ScenarioSession(IStatUI statUI, ScenarioInfo scenarioInfo) {
        score = 0;
        hits = 0;
        misses = 0;
        kills = 0;
        this.statUI = statUI;
        this.scenarioInfo = scenarioInfo;
    }

    public void SetPlayMode(ScenarioPlayMode mode) {
        playMode = mode;
    }

    public void SetTimeAllowed(int timeAllowed) {
        this.timeAllowed = timeAllowed;
    }

    public void SetAmountToKill(int amountToKill) {
        this.amountToKill = amountToKill;
    }

    public void SetScoreSystem(IScoreSystem scoreSystem) {
        scoreSystem.onScoreChanged.AddListener(ChangeScore);
        this.scoreSystem = scoreSystem;
    }

    public void SetHitMissDeterminer(IHitMissDeterminer determiner) {
        determiner.onHit.AddListener(RegisterHit);
        determiner.onMiss.AddListener(RegisterMiss);
        this.hitMissDeterminer = determiner;
    }

    public void SetKillNotifier(UnityEvent onKill) {
        onKill.AddListener(RegisterKill);
        this.onKillEvent = onKill;
    }

    public void SetTimerEvent(TimerTickedEvent e) {
        e.AddListener(UpdateTimeText);
        this.timerTickedEvent = e;
    }

    private void RegisterHit() {
        hits++;
        OnAccuracyChange();
    }

    private void RegisterMiss() {
        misses++;
        OnAccuracyChange();
    }

    private void RegisterKill() {
        kills++;
    }

    private void ChangeScore(float amount) {
        score = Mathf.Max(score + amount, 0); //So can't be negative
        OnScoreChange();
    }

    private void UpdateTimeText(long milliseconds) {
        statUI.UpdateTime(milliseconds);
    }

    #region events
    private void OnAccuracyChange() {
        statUI.UpdateAccuracy(accuracy);
    }

    private void OnScoreChange() {
        statUI.UpdateScore(score);
    }
    #endregion
    
    //This is retarded
    public void UnregisterAllEvents() {
        scoreSystem?.onScoreChanged.RemoveListener(ChangeScore);
        hitMissDeterminer?.onHit.RemoveListener(RegisterHit);
        hitMissDeterminer?.onMiss.RemoveListener(RegisterMiss);
        onKillEvent?.RemoveListener(RegisterKill);
        timerTickedEvent?.RemoveListener(UpdateTimeText);
    }

    public string GetIdentifier() {
        switch (playMode) {
            case ScenarioPlayMode.Timed:
                return scenarioInfo.GenerateTimedIdentifier(timeAllowed);
            case ScenarioPlayMode.TimeTrial:
                return scenarioInfo.GenerateTimeTrialIdentifier(amountToKill);
            default:
                throw new System.Exception("Something went wrong in GetIdentifier! (is ther a playmode that isn't assigned)");
        }
    }
}