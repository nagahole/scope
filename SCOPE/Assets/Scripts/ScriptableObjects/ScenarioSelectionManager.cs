using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new ScenarioSelectionManager", menuName ="ScriptableObjects/Singletons/ScenarioSelectionManager")]
public class ScenarioSelectionManager : ScriptableObject, ICanBeSingleton, IInitializable
{
    [SerializeField] private ScenarioSelectionOptions selectionsPrefab;

    private ScenarioSelectionOptions selection;

    public void Initialize() {
        selection = Instantiate(selectionsPrefab);
        selection.Hide();
    }

    public ScenarioSelectionOptions GetSelection() {
        return selection;
    }
}
