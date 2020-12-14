using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class ScenarioIntParameters : SerializableDictionaryBase<string, int> { }
[System.Serializable]
public class ScenarioFloatParameters : SerializableDictionaryBase<string, float> { }

[System.Serializable]
public struct ScenarioParameters
{
    public ScenarioIntParameters intParameters;
    public ScenarioFloatParameters floatParameters;
}
