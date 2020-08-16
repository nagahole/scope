using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

public class ScenarioInfoEditor : EditorWindow
{
    private KeySceneReferences keyceneReferences;
    private ScenarioInfoContainer scenarioInfoContainer;
    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    private string validationInfo;

    [MenuItem("Window/Scenario Info Editor")]
    public static void ShowWindow() {
        GetWindow<ScenarioInfoEditor>("Scenario Info Editor");
    }

    private void OnGUI() {
        GUILayout.Label("ScenarioInfo Container", EditorStyles.boldLabel);

        scenarioInfoContainer = (ScenarioInfoContainer) EditorGUILayout.ObjectField(scenarioInfoContainer,typeof(ScenarioInfoContainer));

        

        GUILayout.Label("Last load time (ms):");
        GUILayout.Label(stopwatch.ElapsedMilliseconds.ToString());

        GUILayout.Label("Last load time (ticks):");
        GUILayout.Label(stopwatch.ElapsedTicks.ToString());

        if (GUILayout.Button("Add scenes to build")) {
            stopwatch.Restart();
            AddScenesToBuild();
            stopwatch.Stop();
        }

        if (GUILayout.Button("Clear Scenes")) {
            stopwatch.Restart();
            ClearScenesFromBuild();
            stopwatch.Stop();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Scene Navigation Helper", EditorStyles.boldLabel);

        keyceneReferences = (KeySceneReferences)EditorGUILayout.ObjectField(keyceneReferences, typeof(KeySceneReferences));

        if (GUILayout.Button("Go to Menu Scene")) {
            EditorSceneManager.OpenScene(keyceneReferences.menuPath);
        }

        if (GUILayout.Button("Play from menu")) {
            EditorSceneManager.OpenScene(keyceneReferences.menuPath);
            EditorApplication.isPlaying = true;
        }

        GUILayout.Label("Goto Scenarios");

        if(scenarioInfoContainer != null) {
            for (int i = 0; i < scenarioInfoContainer.scenarios.Length; i++) {
                if (GUILayout.Button(scenarioInfoContainer.scenarios[i].name)) {
                    EditorSceneManager.OpenScene(scenarioInfoContainer.scenarios[i].scenePath);
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Scenerio Validator", EditorStyles.boldLabel);

        if (GUILayout.Button("Validate this scene")) {
            ValidateScene();
        }

        GUILayout.Label(validationInfo);
    }

    private void AddScenesToBuild() {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
            scenes.Add(EditorBuildSettings.scenes[i]);
        }

        for (int i = 0; i < scenarioInfoContainer.scenarios.Length; i++) {
            if (!System.Array.Exists(EditorBuildSettings.scenes, (x) => x.path == scenarioInfoContainer.scenarios[i].scenePath)) {
                scenes.Add(new EditorBuildSettingsScene(scenarioInfoContainer.scenarios[i].scenePath, true));
            }
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private void ClearScenesFromBuild() {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
            scenes.Add(EditorBuildSettings.scenes[i]);
        }

        for (int i = 0; i < scenarioInfoContainer.scenarios.Length; i++) {
            
            for (int i2 = scenes.Count - 1; i2 >= 0; i2--) {
                if(scenes[i2].path == scenarioInfoContainer.scenarios[i].scenePath) {
                    scenes.RemoveAt(i2);
                }
            }
            
        }

        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private void ValidateScene() {
        validationInfo = string.Empty;

        if(FindObjectOfType<PlayerInfoHandler>() == null) {
            validationInfo += $"No {typeof(PlayerInfoHandler).Name} found\n";
        }

        if (NagaUnityUtilities.NagaUtils.FindObjectOfType<IScoreSystem>() == null) {
            validationInfo += $"No {typeof(IScoreSystem).Name} found\n";
        }

        if(validationInfo == string.Empty) {
            validationInfo = "Looks good to me! :)";
        }
    }
}
