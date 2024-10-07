using UnityEngine;

namespace StateTree
{
    public class StateTreeMB : MonoBehaviour
    {
        [SerializeField] protected RootState.Data rootStateData;
        protected RootState rootState;
        public RootState RootState => rootState;

        [ContextMenu("Print Current States")]
        void printCurrentStates() => rootState.PrintCurrentStates();

        protected virtual void Update() => rootState.UpdateStateTree();
        protected virtual void FixedUpdate() => rootState.FixedUpdateStateTree();
        protected virtual void OnDestroy() => rootState.DestroyStateTree();

        //protected virtual void OnDrawGizmos() => rootState.OnDrawGizmos();
        //protected virtual void OnDrawGizmosSelected() => rootState.OnDrawGizmosSelected();

        // Collisions
        protected virtual void OnCollisionEnter(Collision collision) => rootState.OnCollisionEnter?.Invoke(collision);
        protected virtual void OnCollisionStay(Collision collision) => rootState.OnCollisionStay?.Invoke(collision);
        protected virtual void OnCollisionExit(Collision collision) => rootState.OnCollisionExit?.Invoke(collision);
        protected virtual void OnTriggerEnter(Collider collider) => rootState.OnTriggerEnter?.Invoke(collider);
        protected virtual void OnTriggerStay(Collider collider) => rootState.OnTriggerStay?.Invoke(collider);
        protected virtual void OnTriggerExit(Collider collider) => rootState.OnTriggerExit?.Invoke(collider);
        protected virtual void OnCollisionEnter2D(Collision2D collision) => rootState.OnCollisionEnter2D?.Invoke(collision);
        protected virtual void OnCollisionStay2D(Collision2D collision) => rootState.OnCollisionStay2D?.Invoke(collision);
        protected virtual void OnCollisionExit2D(Collision2D collision) => rootState.OnCollisionExit2D?.Invoke(collision);
        protected virtual void OnTriggerEnter2D(Collider2D collision) => rootState.OnTriggerEnter2D?.Invoke(collision);
        protected virtual void OnTriggerStay2D(Collider2D collision) => rootState.OnTriggerStay2D?.Invoke(collision);
        protected virtual void OnTriggerExit2D(Collider2D collision) => rootState.OnTriggerExit2D?.Invoke(collision);
    }
}
