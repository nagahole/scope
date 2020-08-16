using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class SOSingleton<T> where T : ScriptableObject, ICanBeSingleton
{
    private static T _sharedInstance;
    public static T sharedInstance {
        get {
            Initialize();
            return _sharedInstance;
        }
    }

    public static void Initialize() {
        if (_sharedInstance == null) {
            _sharedInstance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            if (_sharedInstance == null) {
                throw new System.Exception("Cannot locate scriptable object of type " + typeof(T).Name);
            }
            if (_sharedInstance is IInitializable) {
                (_sharedInstance as IInitializable).Initialize();
            }
        }
    }
}
