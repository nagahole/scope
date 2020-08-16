using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;

/*
 * 
 * 
 *  DEPENDENCIES REQUIRED
 *      -MEC
 *      -UNITYENGINE
 * 
 * 
 * 
 */

namespace NagaUnityUtilities 
{
    public class NagaUtils {
        /// <summary>
        /// Similar to unities version. However, this supports finding interfaces. NOTE: THIS IS VERY RESOURCE HEAVY, AND SHOULD ONLY BE USED IN ONVALIDATE FUNCTIONS
        /// </summary>
        public static T[] FindObjectsOfType<T>() {
            List<T> retVal = new List<T>();
            foreach (T i in Object.FindObjectsOfType<Object>().OfType<T>()) {
                retVal.Add(i);
            }
            return retVal.ToArray();
        }

        /// <summary>
        /// Similar to unities version. However, this supports finding interfaces. NOTE: THIS IS VERY RESOURCE HEAVY, AND SHOULD ONLY BE USED IN ONVALIDATE FUNCTIONS
        /// </summary>
        public static T FindObjectOfType<T>() {
            T[] objects = FindObjectsOfType<T>();
            if (objects.Length == 0) {
                return default;
            }
            return objects[0];
        }

        public static void ExecuteAfterOneFrame(System.Action action) {
            Timing.RunCoroutine(_ExecuteAfterOneFrame(action));
        }

        private static IEnumerator<float> _ExecuteAfterOneFrame(System.Action action) {
            yield return Timing.WaitForOneFrame;
            action();
        }

        public static void YieldUntilCondition(System.Func<bool> condition, System.Action action) {
            Timing.RunCoroutine(_YieldUntilCondition(condition, action));
        }

        private static IEnumerator<float> _YieldUntilCondition(System.Func<bool> condition, System.Action action) {
            while (!condition()) {
                yield return Timing.WaitForOneFrame;
            }
            action?.Invoke();
        }

        public static void ExecuteAfterSeconds(System.Action action, float seconds) {
            Timing.RunCoroutine(_ExecuteAfterSeconds(action, seconds));
        }

        private static IEnumerator<float> _ExecuteAfterSeconds(System.Action action, float seconds) {
            yield return Timing.WaitForSeconds(seconds);
            action?.Invoke();
        }

        public static Vector3 GetPointAlongDirection(Vector3 origin, Vector3 direction, float length) {
            return direction.normalized * length + origin;
        }

        public static Vector3 RotateVector(Vector3 vector, Vector3 eulerAngles) {
            return Quaternion.Euler(eulerAngles) * vector;
        }

        public static Vector3 RotateVectorAboutPoint(Vector3 vector, Vector3 rotation, Vector3 point) {
            return (RotateVector(vector - point, rotation)) + point;
        }

        public static void DrawCuboid(Vector3 center, Vector3 size, Quaternion rotation) {
            //Bottom vertices
            Vector3 v1 = center + rotation * new Vector3(-size.x, -size.y, -size.z) / 2;
            Vector3 v2 = center + rotation * new Vector3(size.x, -size.y, -size.z) / 2;
            Vector3 v3 = center + rotation * new Vector3(size.x, -size.y, size.z) / 2;
            Vector3 v4 = center + rotation * new Vector3(-size.x, -size.y, size.z) / 2;

            //Starting from back left and going in counter clockwise

            //Top vertices
            Vector3 v5 = center + rotation * new Vector3(-size.x, size.y, -size.z) / 2;
            Vector3 v6 = center + rotation * new Vector3(size.x, size.y, -size.z) / 2;
            Vector3 v7 = center + rotation * new Vector3(size.x, size.y, size.z) / 2;
            Vector3 v8 = center + rotation * new Vector3(-size.x, size.y, size.z) / 2;

            //Bottom
            Gizmos.DrawLine(v1, v2);
            Gizmos.DrawLine(v2, v3);
            Gizmos.DrawLine(v3, v4);
            Gizmos.DrawLine(v4, v1);

            //Top
            Gizmos.DrawLine(v5, v6);
            Gizmos.DrawLine(v6, v7);
            Gizmos.DrawLine(v7, v8);
            Gizmos.DrawLine(v8, v5);

            //Inbetween
            Gizmos.DrawLine(v1, v5);
            Gizmos.DrawLine(v2, v6);
            Gizmos.DrawLine(v3, v7);
            Gizmos.DrawLine(v4, v8);
        }

        public static T VerifyGetComponent<T>(Component obj) {
            T retVal = obj.GetComponent<T>();
            if (retVal == null) {
                throw new System.Exception($"{obj.ToString()} variable does not implement interface {typeof(T).Name}");
            }
            return retVal;
        }
    }
}

