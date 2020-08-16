using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScoreChangedEvent : UnityEvent<float> { }
public interface IScoreSystem {
    ScoreChangedEvent onScoreChanged { get; }
    void NotifyPoints(float basePoints);
    void ResetPoints();
}

//This does not handle UI. UI is handled in the ScenarioManager with the ScenarioSession events
public abstract class ScoreSystem : MonoBehaviour, IScoreSystem {
    [SerializeField] private ScoreChangedEvent _onScoreChanged;
    public ScoreChangedEvent onScoreChanged {
        get {
            return _onScoreChanged;
        }
        private set {
            _onScoreChanged = value;
        }
    }

    public abstract void NotifyPoints(float basePoints);
    public abstract void ResetPoints();
}
