using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NagaUnityUtilities;
using MEC;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.Events;

/*
 * 
 * 
 * This class is getting to big
 * 
 * If another mode gets added, refactor it to use polymorphism to manage the different playmodes
 * 
 * 
 */ 
namespace ScenarioManagement 
{
    public enum ScenarioState {
        None,
        ShootToStart,
        Countdown,
        Playing,
        Results,
        Untimed
    }

    public enum ScenarioPlayMode {
        Timed,
        Untimed,
        TimeTrial
    }

    [CreateAssetMenu(fileName = "new ScenarioManager", menuName = "ScriptableObjects/Singletons/ScenarioManager")]
    public class ScenarioManager : ScriptableObject, IInitializable, ICanBeSingleton {
        [SerializeField] private ScenarioManager self;

        [SerializeField] private int countdownTime = 3;
        [SerializeField] private Component statUIPrefab; //IStatUI
        [SerializeField] private Component sessionResultsUIPrefab; //IResultsUI
        [SerializeField] private Component countdownUIPrefab; //ICountdownUI

        [SerializeField] private ScriptableObject highscoreTrackerObject;
        [SerializeField] private ScenarioTimer scenarioTimer; //May make this into an interface in the future, but it is fine as is right now

        [SerializeField] private UnityEvent _onScenarioStarted;
        public UnityEvent onScenarioStarted => _onScenarioStarted;

        private ScenarioSession session;

        #region UI cache
        private IStatUI statUI;
        private ISessionResultsUI sessionResultsUI;
        private ICountdownUI countdownUI;
        #endregion

        public IHighscoreWrapper highscoreTracker { get; private set; }

        private bool isLoading;
        private bool isPaused;

        private CoroutineHandle shootToStartCoroutine;

        [SerializeField] [ReadOnly] private ScenarioState _scenarioState;

        public ScenarioState scenarioState => _scenarioState;

        public void Initialize() {
            isLoading = false;
            isPaused = false;
            SetupUI(ref statUI, statUIPrefab);
            SetupUI(ref sessionResultsUI, sessionResultsUIPrefab);
            SetupUI(ref countdownUI, countdownUIPrefab);

            highscoreTracker = highscoreTrackerObject as IHighscoreWrapper;
            if (highscoreTracker == null) {
                throw new System.Exception("HighscoreTrackerObject does not inherit " + typeof(IHighscoreWrapper).Name);
            }

            SOSingleton<IngameMenuHandler>.Initialize();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            statUI.Hide();
            sessionResultsUI.Hide();
            countdownUI.Hide();
        }

        private void OnSceneUnloaded(Scene arg0) {
            StopScenario();
        }

        private void SetupUI<T>(ref T fieldMember, Component prefab) {
            fieldMember = NagaUtils.FindObjectOfType<T>();
            if (fieldMember == null) {
                var obj = Instantiate(prefab);
                DontDestroyOnLoad(obj.gameObject);
                fieldMember = obj.GetComponent<T>();

                if (fieldMember == null) {
                    throw new System.Exception($"Prefab does not contain a component that implements {typeof(T).Name}!");
                }
            }
        }

        public void StopScenario() {
            scenarioTimer.ClearEvents();
            scenarioTimer.StopAll();
            KillNotifier.onKill.RemoveListener(OnTimeTrialKill); 
            _scenarioState = ScenarioState.None;
            if (shootToStartCoroutine != null) {
                Timing.KillCoroutines(shootToStartCoroutine);
            }
        }

        public void PlayScenarioTimed(ScenarioInfo scenario, int timeAllowed) {
            if (isLoading) {
                return;
            }

            PreLoadScenario();

            LoadScenario(scenario, () => {
                BaseOnScenarioLoaded(scenario);
                session.SetPlayMode(ScenarioPlayMode.Timed);
                session.SetTimeAllowed(timeAllowed);
                TimedOnScenarioLoaded();
                
            });
        }

        public void PlayScenarioUntimed(ScenarioInfo scenario) {
            if (isLoading) {
                return;
            }

            PreLoadScenario();

            LoadScenario(scenario, () => {
                BaseOnScenarioLoaded(scenario);
                UntimedOnScenarioLoaded();
            });
        }

        public void PlayScenarioTimeTrial(ScenarioInfo scenario, int amountToKill) {
            if (isLoading)
                return;

            PreLoadScenario();

            LoadScenario(scenario, () => {
                BaseOnScenarioLoaded(scenario);
                session.SetPlayMode(ScenarioPlayMode.TimeTrial);
                session.SetAmountToKill(amountToKill);
                TimeTrialOnScenarioLoaded(amountToKill);
            });
        }

        /// <summary>
        /// Automatically plays the last played scenario with their settings
        /// </summary>
        public void ReplayScenario() {
            Debug.Log(session.playMode);
            if(session.playMode == ScenarioPlayMode.Timed) {
                ReplayScenarioTimed(session.timeAllowed);
            } else if (session.playMode == ScenarioPlayMode.TimeTrial) {
                ReplayScenarioTimeTrial(session.amountToKill);
            } else {
                Debug.LogError("Error in ReplayScenario. Is it because some playmode isn't accounted for?");
            }
        }

        public void ReplayScenarioTimeTrial(int amountToKill) {
            PlayScenarioTimeTrial(session.scenarioInfo, amountToKill);
        }

        public void ReplayScenarioTimed(int timeAllowed) {
            PlayScenarioTimed(session.scenarioInfo, timeAllowed);
        }

        public void ReplayScenarioUntimed() {
            PlayScenarioUntimed(session.scenarioInfo);
        }

        private void PreLoadScenario() {
            scenarioTimer.ClearEvents();
            scenarioTimer.StopTimer();
        }

        private void BaseOnScenarioLoaded(ScenarioInfo info) {
            CreateNewSession(info);

            sessionResultsUI.Hide();

            SetupStatUI();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UntimedOnScenarioLoaded() {
            _scenarioState = ScenarioState.Untimed;

            session.SetScoreSystem(NagaUtils.FindObjectOfType<IScoreSystem>());

            EnablePlayer();
            statUI.SetUntimed();
            countdownUI.Hide();
            onScenarioStarted.Invoke();
        }

        private void TimedOnScenarioLoaded() {
            _scenarioState = ScenarioState.ShootToStart;

            session.SetScoreSystem(NagaUtils.FindObjectOfType<IScoreSystem>());

            SetupCountdownLogic();
            statUI.SetTimeDP(0);
            statUI.SetTimerMode();
            statUI.UpdateTime(session.timeAllowed * 1000);
            SetupCountdownUI();

            scenarioTimer.onEndTimer.AddListener(OnTimedScenarioEnded);

            shootToStartCoroutine = Timing.RunCoroutine(_WaitToStart(()=> {
                scenarioTimer.StartTimer(session.timeAllowed, countdownTime);
                _scenarioState = ScenarioState.Countdown;
            }));
        }

        private void TimeTrialOnScenarioLoaded(int amountToKill) {
            _scenarioState = ScenarioState.ShootToStart;

            SetupCountdownLogic();
            statUI.SetTimeDP(2);
            statUI.SetStopwatchMode();
            statUI.UpdateTime(0);
            statUI.UpdateScoreText(string.Join("/", 0, amountToKill));
            SetupCountdownUI();

            TrackKills(amountToKill);

            shootToStartCoroutine = Timing.RunCoroutine(_WaitToStart(()=> {
                scenarioTimer.StartStopwatch(countdownTime);
                _scenarioState = ScenarioState.Countdown;
            }));
        }

        private void SetupCountdownLogic() {
            PlayerInfoHandler.sharedInstance.GetInventoryService().DisableWeapon();
            scenarioTimer.onStartTimer.AddListener(PlayerInfoHandler.sharedInstance.GetInventoryService().EnableWeapon);
            scenarioTimer.onStartTimer.AddListener(() => { _scenarioState = ScenarioState.Playing; });
            scenarioTimer.onStartTimer.AddListener(onScenarioStarted.Invoke);
        }

        private void TrackKills(int amountToKill) {
            timeTrialKills = 0;
            this.amountToKill = amountToKill;
            KillNotifier.onKill.AddListener(OnTimeTrialKill);
        }

        //What is this garbage - needs refactoring and polymorphism

        private int timeTrialKills;
        private int amountToKill;

        private void OnTimeTrialKill() {
            timeTrialKills++;
            statUI.UpdateScoreText(string.Join("/", timeTrialKills, amountToKill));
            if(timeTrialKills >= amountToKill) {
                TimeTrialOnScenarioEnded();
                KillNotifier.onKill.RemoveListener(OnTimeTrialKill);
            }
        }

        private IEnumerator<float> _WaitToStart(System.Action callback) {
            while (true) {
                if (Input.GetButtonDown("Fire1") && !isPaused)
                    break;
                yield return Timing.WaitForOneFrame;
            }
            callback?.Invoke();
        }

        private void OnTimedScenarioEnded() {
            DisableMovementAtTheEnd();

            SetupBaseSessionResults();
            sessionResultsUI.SetTitle(string.Join(" - ", session.scenarioInfo.name, session.timeAllowed.ToString() + "s"));

            sessionResultsUI.SetScore(session.score);
            sessionResultsUI.SetHighscore(highscoreTracker.TryGetHighscore(session).score);

            sessionResultsUI.SetScoreDescriber("Score");
            sessionResultsUI.SetHighscoreDescriber("Highscore");

            //Registers high score after setting up session results makes highscore lower then score if the score beats the highscore - change order if want to change this
            bool isHighscore = highscoreTracker.RegisterScore(session, session.score, session.scenarioInfo.scoreMode);
            sessionResultsUI.IsNewHighscore(isHighscore);
        }

        private void TimeTrialOnScenarioEnded() {
            DisableMovementAtTheEnd();

            SetupBaseSessionResults();
            sessionResultsUI.SetTitle(string.Join(" - ", session.scenarioInfo.name, session.amountToKill.ToString() + " kills"));

            float time = scenarioTimer.StopStopwatch();

            sessionResultsUI.SetScoreDescriber("Time");
            sessionResultsUI.SetHighscoreDescriber("Best Time");
            sessionResultsUI.SetScoreText(time.ToMinutesAndSecondsFormatted(2));

            var best = highscoreTracker.TryGetHighscore(session);
            if(!best.exists) //haven't done before - deafult value
                sessionResultsUI.SetHighscoreText("-/-");
            else {
                sessionResultsUI.SetHighscoreText(best.score.ToMinutesAndSecondsFormatted(2));
            }

            //Doesn't use the scenarioinfo's scoremode because this is timed, and lower time always better
            bool isHighscore = highscoreTracker.RegisterScore(session, time, ScoreMode.LowerIsBetter);
            sessionResultsUI.IsNewHighscore(isHighscore);
        }

        private void DisableMovementAtTheEnd() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _scenarioState = ScenarioState.Results;

            DisablePlayer();
        }

        private void SetupBaseSessionResults() {
            sessionResultsUI.Show();
            sessionResultsUI.Setup();

            //doing this because when playing time trial, on the last kill, the session ends before the kill has finistered registering, ending up with 1 less kill than there is supposed to be
            NagaUnityUtilities.NagaUtils.ExecuteAfterOneFrame(() => { //optimise this into a class method if too bad
                sessionResultsUI.SetAccuracy(session.hits, session.misses);
                sessionResultsUI.SetKills(session.kills);
            });
        }

        private void CreateNewSession(ScenarioInfo info) {
            session?.UnregisterAllEvents();

            //session.SetScoreSystem(NagaUtils.FindObjectOfType<IScoreSystem>()); This got removed because the TimeTrial mode doesn't include score
            session = new ScenarioSession(statUI, info);
            session.SetHitMissDeterminer(NagaUtils.FindObjectsOfType<IHitMissDeterminer>()[0]); //maybe have a global notifier like KillNotifier? TODO;
            session.SetTimerEvent(scenarioTimer.onTimerTicked);
            session.SetKillNotifier(KillNotifier.onKill);
        }

        private void SetupStatUI() {
            statUI.Show();
            statUI.UpdateAccuracy(0);
            statUI.UpdateScore(0);
            statUI.UpdateTime(session.timeAllowed * 1000);
            statUI.Setup();
        }

        private void SetupCountdownUI() {
            countdownUI.Show();
            countdownUI.SetStartingCountdownTime(countdownTime);
            scenarioTimer.onCountdownTicked.AddListener(countdownUI.CountdownTicked);
            countdownUI.Setup();
        }

        #region loading
        public delegate void onSceneLoaded();

        private void LoadScenario(ScenarioInfo scenario, onSceneLoaded callback) {
            Timing.RunCoroutine(_LoadScenario(scenario, callback));
        }

        private IEnumerator<float> _LoadScenario(ScenarioInfo scenario, onSceneLoaded callback) {
            Debug.Log($"Loading Scenario {scenario.scenarioName}");
            isLoading = true;
            yield return Timing.WaitUntilDone(SceneManager.LoadSceneAsync(scenario.scenePath, LoadSceneMode.Single));

            if (scenario.map != null) {
                var map = Instantiate(scenario.map);
                map.transform.position = scenario.mapPos;
            }

            isLoading = false;
            callback?.Invoke();
        }
        #endregion
        public void Pause() {
            Debug.Log("Pause");
            Time.timeScale = 0;
            
            DisablePlayer();
            isPaused = true;
        }

        public void Resume() {
            Debug.Log("Resume");
            Time.timeScale = 1;

            if (_scenarioState == ScenarioState.Playing || _scenarioState == ScenarioState.Untimed) {
                EnablePlayer();
            }

            if(PlayerInfoHandler.sharedInstance != null) {
                PlayerInfoHandler.sharedInstance.GetMouseIOService().Unlock();
            }

            isPaused = false;
        }

        private void DisablePlayer() {
            if(PlayerInfoHandler.sharedInstance != null) {
                PlayerInfoHandler.sharedInstance.GetMouseIOService().Lock();
                PlayerInfoHandler.sharedInstance.GetInventoryService().DisableWeapon();
            }
        }

        private void EnablePlayer() {
            if (PlayerInfoHandler.sharedInstance != null) {
                PlayerInfoHandler.sharedInstance.GetMouseIOService().Unlock();
                PlayerInfoHandler.sharedInstance.GetInventoryService().EnableWeapon();
            }
        }
    }

}
