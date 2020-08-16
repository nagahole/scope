using UnityEngine;

namespace NagaUnityUtilities
{
    public class ObjectPoolInstance : MonoBehaviour {
        [SerializeField] [ReadOnly] private int prefabId;

        private bool setup = true;

        internal void SetID(int id) {
            prefabId = id;
        }

        internal int GetID() {
            return this.prefabId;
        }

        //Incase i want specific implementations that does not do this
        protected virtual void OnDisable() {
            if (setup) {
                setup = false;
                return;
            }
            Repool();
        }

        protected void Repool() {
            GenericObjectPooler.RepoolObject(this);
        }
    }
}
