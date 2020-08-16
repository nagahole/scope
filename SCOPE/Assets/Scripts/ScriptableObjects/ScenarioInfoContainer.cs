using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ScenarioInfoContainer", menuName = "ScriptableObjects/ScenarioInfoContainer")]
public class ScenarioInfoContainer : ScriptableObject
{
    [SerializeField] private ScenarioInfo[] _scenarios;

    public ScenarioInfo[] scenarios => _scenarios;
}
