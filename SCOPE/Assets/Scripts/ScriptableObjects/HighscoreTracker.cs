using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NagaUnityUtilities;
using RotaryHeart;

public enum ScoreMode {
    HigherIsBetter,
    LowerIsBetter
}
public interface IHighscoreWrapper {
    bool RegisterScore(ScenarioSession session, float score, ScoreMode scoreMode);
    (bool exists, float score) TryGetHighscore(ScenarioSession session);
    (bool exists, float score) TryGetHighscore(string id);
    void Purge();
}
[CreateAssetMenu(fileName = "new HighscoreTracker", menuName="ScriptableObjects/HighscoreTracker")]
public class HighscoreTracker : ScriptableObject, IHighscoreWrapper
{
    [System.Serializable]
    private class HighscoreData {
        //Hash, float
        public Dictionary<string, float> highScores;//

        public HighscoreData(Dictionary<string, float> dict) {
            highScores = dict;
        }
    }

    private const string RELATIVE_FILE_PATH = "/highscores.megagay";

    private string filePath;

    [SerializeField] [ReadOnly] private HighscoreData data = new HighscoreData(new Dictionary<string, float>());

    private BinaryFormatter formatter = new BinaryFormatter();

    private void OnEnable() {
        filePath = Application.persistentDataPath + RELATIVE_FILE_PATH;
        LoadAll();
    }

    /// <summary>
    /// Returns true if registered score is new high score
    /// </summary>
    /// <param name="session"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool RegisterScore(ScenarioSession session, float score, ScoreMode scoreMode) {
        string id = session.GetIdentifier();
        Debug.Log(id);
        bool isHighscore = false;

        if (!data.highScores.ContainsKey(id)) {
            data.highScores.Add(id, score);
            isHighscore = true;
            SaveAll();
            Debug.Log("No previous score, so new high score!!");
        } else {
            float highScore = data.highScores[id];
            if((highScore < score && scoreMode == ScoreMode.HigherIsBetter) ||
                (highScore > score && scoreMode == ScoreMode.LowerIsBetter)) {

                data.highScores[id] = score;
                isHighscore = true;
                SaveAll();
                Debug.Log("New highscore!!");
            }
        }
        
        return isHighscore;
    }

    public (bool exists, float score) TryGetHighscore(ScenarioSession session) {
        string id = session.GetIdentifier();
        return TryGetHighscore(id);
    }

    public (bool exists, float score) TryGetHighscore(string id) {
        Debug.Log(id);
        if (!data.highScores.ContainsKey(id)) {
            return (false, 0);
        } else {
            return (true, data.highScores[id]);
        }
    }

    public void Purge() {
        Debug.Log("Purging Highscores");
        data = new HighscoreData(new Dictionary<string, float>());
        SaveAll();
    }

    private void SaveAll() {
        try {
            Debug.Log("saving highscore...");
            using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
                formatter.Serialize(stream, data);
            }
            Debug.Log("Saving done");
        } catch (System.Exception e) {
            Debug.LogError(e.Message);
        }
    }

    private void LoadAll() {
        if (File.Exists(filePath)) {
            try {
                using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
                    data = formatter.Deserialize(stream) as HighscoreData;
                }
            } catch (System.Exception e){
                Debug.Log($"Unable to load highscore file. Defaulting values | Error: {e.Message}");
            }
            
        } else {
            Debug.LogWarning("Highscore file not found");
        }
    }
}


