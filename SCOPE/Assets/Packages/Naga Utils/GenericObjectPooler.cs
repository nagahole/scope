#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * 
 *  HOW TO USE:
 *  
 *  Attach the ObjectPoolInstance MonoBehaviour to any object that is to be pooled, and create the pool of the prefab
 *  on Start() (Not awake()).
 *  
 *  To get the object, use the RequestObject method
 *  
 *  Instead of destroying, just gameObject.SetActive(false) 
 *  
 *  On disabling, this will automatically repool the object
 * 
 */

namespace NagaUnityUtilities 
{
    public static class GenericObjectPooler {

        private struct ObjectPool {
            public Transform parent;
            public Queue<ObjectPoolInstance> queue;

            public ObjectPool(Transform parent, Queue<ObjectPoolInstance> queue) {
                this.parent = parent;
                this.queue = queue;
            }
        }

        private static Transform mainParent;
        private static Dictionary<int, ObjectPool> objectPoolDictionary = new Dictionary<int, ObjectPool>();

        private const string containerName = "Generic Object Pooler";

        static GenericObjectPooler() {
            FindMainParent();
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            FindMainParent();
        }

        private static void OnSceneUnloaded(Scene arg0) {
            objectPoolDictionary.Clear();
        }

        private static void FindMainParent() {
            var obj = GameObject.Find(containerName);
            if (obj == null) {
                mainParent = new GameObject(containerName).transform;
            } else {
                mainParent = obj.transform;
            }
        }

        public static void CreatePool(ObjectPoolInstance prefab, int amount) {
            int id = prefab.GetInstanceID();
            if (!objectPoolDictionary.ContainsKey(id)) {
                //Debug.Log($"Creating pool of {prefab.name} ({id})");
                Transform poolParent = new GameObject(prefab.name).transform;
                poolParent.parent = mainParent;

                Queue<ObjectPoolInstance> queue = new Queue<ObjectPoolInstance>();

                for (int i = 0; i < amount; i++) {
                    var newObj = Object.Instantiate(prefab);
                    newObj.transform.parent = poolParent;
                    newObj.gameObject.SetActive(true);
                    newObj.gameObject.SetActive(false);
                    newObj.SetID(id);

                    queue.Enqueue(newObj);
                }

                ObjectPool objectPool = new ObjectPool(poolParent, queue);

                objectPoolDictionary.Add(id, objectPool);
                return;
            } else {
                Debug.LogWarning($"Attempt to make another pool of existing pool of {prefab.gameObject.name}. Instead adding more to the pool");
                var objectPool = objectPoolDictionary[id];

                for (int i = 0; i < amount; i++) {
                    var newObj = Object.Instantiate(prefab);
                    newObj.transform.parent = objectPool.parent;
                    newObj.gameObject.SetActive(true);
                    newObj.gameObject.SetActive(false);

                    objectPool.queue.Enqueue(newObj);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="returnActive">Returns the object active or not, active by default</param>
        /// <returns></returns>
        public static ObjectPoolInstance RequestObject(ObjectPoolInstance prefab, bool returnActive) {
            int id = prefab.GetInstanceID();
            //Debug.Log($"Requesting pool of {prefab.name} ({id})");

            if (objectPoolDictionary.ContainsKey(id)) {
                var pool = objectPoolDictionary[id];
                ObjectPoolInstance instance;
                if (pool.queue.Count > 0) {
                    instance = pool.queue.Dequeue();
                } else { //CREATE NEW INSTANCE
                    var newObj = Object.Instantiate(prefab);
                    newObj.gameObject.SetActive(true);
                    newObj.SetID(id);
                    newObj.transform.parent = pool.parent;

                    instance = newObj;
                }

                instance.gameObject.SetActive(returnActive);

                return instance;

            }

            //Bruh what is this

            throw new System.Exception($"{typeof(GenericObjectPooler).Name} does not contain a pool for {prefab.name} ({id}!)");
        }

        /// <summary>
        /// Returns the active gameobject
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static ObjectPoolInstance RequestObject(ObjectPoolInstance prefab) {
            return RequestObject(prefab, true);
        }

        public static void RepoolObject(ObjectPoolInstance instance) {
            int id = instance.GetID();
            if (objectPoolDictionary.ContainsKey(id)) {
                objectPoolDictionary[id].queue.Enqueue(instance);
                return;
            }

            Debug.Log(instance.gameObject);
            throw new System.Exception($"{typeof(GenericObjectPooler).Name} does not contain a pool for ({id}!)");
        }
    }
}
