#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MEC;
using NagaUnityUtilities;
using System.Timers;
using UnityEngine.Events;
using System.Linq;
using OdinSerializer;

/// <summary>
/// One per scene
/// </summary>
namespace ScenarioManagement 
{
    [System.Serializable]
    //Milliseconds remaining
    public class TimerTickedEvent : UnityEvent<long> { }

    [CreateAssetMenu(fileName ="new ScenarioTimer", menuName = "ScriptableObjects/Scenario Timer")]
    public class ScenarioTimer : ScriptableObject {
        public readonly UnityEvent onStartCountdown = new UnityEvent(),
            onEndCountdown = new UnityEvent(),

            //Despite the unconventional name, stopwatch and timers are going to use the same event
            onStartTimer = new UnityEvent(),
            onEndTimer = new UnityEvent();

        public readonly TimerTickedEvent onTimerTicked = new TimerTickedEvent(),
            onCountdownTicked = new TimerTickedEvent();

        [SerializeField] private ScenarioTimer self;
        [SerializeField][ReadOnly] private float time;

        private bool isAlreadyRunning = false;

        private CoroutineHandle countdownCoroutine, timerCoroutine;

        public bool countdownActive { get; private set; }

        public delegate void onFinishedTimer();

        private void OnEnable() {
            isAlreadyRunning = false;
        }

        //Subscribers should unregister themselves
        public void ClearEvents() {
            onStartCountdown.RemoveAllListeners();
            onEndCountdown.RemoveAllListeners();
            onStartTimer.RemoveAllListeners();
            onEndTimer.RemoveAllListeners();
            onTimerTicked.RemoveAllListeners();
        }

        public void StopTimer() {
            isAlreadyRunning = false;
            StopCountdownCoroutine();
            StopTimerCoroutine();
        }

        private void StopCountdownCoroutine() {
            if (countdownCoroutine != null) {
                Timing.KillCoroutines(countdownCoroutine);
            }
        }

        private void StopTimerCoroutine() {
            if (timerCoroutine != null) {
                Timing.KillCoroutines(timerCoroutine);
            }
        }

        public void StopAll() {
            StopTimer();
            StopCountdownCoroutine();
            StopTimerCoroutine();
        }

        public bool StartTimer(int timeInSeconds, int countdownTimeInSeconds) {
            if (isAlreadyRunning) {
                Debug.Log("Is already running");
                return false;
            }

            timerCoroutine = Timing.RunCoroutine(_StartTimer(timeInSeconds, countdownTimeInSeconds));
            return true;
        }

        public bool StartStopwatch(int countdownTimeInSeconds) {
            if (isAlreadyRunning) {
                Debug.Log("Is already running");
                return false;
            }

            timerCoroutine = Timing.RunCoroutine(_StartStopwatch(countdownTimeInSeconds));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Seconds on timer</returns>
        public float StopStopwatch() {
            if (!isAlreadyRunning || timerCoroutine == null)
                Debug.LogWarning("StopStopwatch is called even though there is no timercoroutine and it isn't already running");

            StopTimerCoroutine();
            onTimerTicked.Invoke((long)(time * 1000));
            return time;
        }

        private IEnumerator<float> _StartTimer(int timeInSeconds, int countdownTimeInSeconds) {
            Debug.Log("STARTING TIMER");
            isAlreadyRunning = true;

            yield return Timing.WaitUntilDone(countdownCoroutine = Timing.RunCoroutine(_PlayCountdown(countdownTimeInSeconds)));

            onStartTimer.Invoke();
            time = timeInSeconds;

            do {
                yield return Timing.WaitForOneFrame;
                time -= Time.deltaTime;
                onTimerTicked.Invoke((long)(time * 1000));
            } while (time > 0);

            onTimerTicked.Invoke((long)(time * 1000));

            onEndTimer.Invoke();
            isAlreadyRunning = false;
        }

        private IEnumerator<float> _StartStopwatch(int countdownTimeInSeconds) {
            Debug.Log("STARTING STOPWATCH");
            isAlreadyRunning = true;

            yield return Timing.WaitUntilDone(countdownCoroutine = Timing.RunCoroutine(_PlayCountdown(countdownTimeInSeconds)));
            onStartTimer.Invoke();
            time = 0;
            while (true) {
                yield return Timing.WaitForOneFrame;
                time += Time.deltaTime;
                onTimerTicked.Invoke((long)(time * 1000));
            }
        }

        private IEnumerator<float> _PlayCountdown(int timeInSeconds) {
            onStartCountdown.Invoke();
            float timeRemaining = timeInSeconds;
            while (true) {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0)
                    break;
                onCountdownTicked.Invoke((long) (timeRemaining * 1000));
                yield return Timing.WaitForOneFrame;
            }
            onCountdownTicked.Invoke(0);
            onEndCountdown.Invoke();
        }
    }
}

