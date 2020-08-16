using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NagaUnityUtilities 
{
    public static class VectorExtensions {
        public static Vector2 RandomNormalized(this Vector2 vector2) {
            return new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized;
        }

        public static Vector3 RandomNormalized(this Vector3 vector2) {
            return new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f).normalized;
        }
    }
}

