using System;
using UnityEngine;

namespace StateTree
{
    public class RootState : State
    {
        public Data data;

        #region Events
        //public Action OnDrawGizmos;
        //public Action OnDrawGizmosSelected;

        public Action<Collision> OnCollisionEnter;
        public Action<Collision> OnCollisionStay;
        public Action<Collision> OnCollisionExit;
        public Action<Collider> OnTriggerEnter;
        public Action<Collider> OnTriggerStay;
        public Action<Collider> OnTriggerExit;
        public Action<Collision2D> OnCollisionEnter2D;
        public Action<Collision2D> OnCollisionStay2D;
        public Action<Collision2D> OnCollisionExit2D;
        public Action<Collider2D> OnTriggerEnter2D;
        public Action<Collider2D> OnTriggerStay2D;
        public Action<Collider2D> OnTriggerExit2D;
        #endregion

        public RootState(Data data, Node child = null) : this(data, null, child) { }
        public RootState(Data data, string name, Node child = null) : base(child)
        {
            this.data = data;
            this.name = name;

            SetupWrapper(this, 0);

            if (data.createGameObjectTree)
                CreateGameObjectTree(null);
        }

        [System.Serializable]
        public class Data
        {
            public bool debugState = false;
            public bool debugGeneral = false;
            public bool createGameObjectTree = false;
        }

        public void UpdateStateTree()
        {
            Evaluate();

            UpdateStateWrapper();
        }

        public void FixedUpdateStateTree() => FixedUpdateStateWrapper();
        public void DestroyStateTree() => DestroyWrapper();
    }
}
