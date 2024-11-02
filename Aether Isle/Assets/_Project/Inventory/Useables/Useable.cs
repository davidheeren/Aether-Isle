using Game;
using UnityEngine;

namespace Inventory
{
    public abstract class Useable
    {
        public readonly UseableData baseData;
        protected readonly ActorComponents components;
        private ToolData toolData;

        public Useable(UseableData data, ActorComponents components)
        {
            baseData = data;
            this.components = components;
        }

        public virtual void Equip() { }// Debug.Log($"Equipped: {baseData.id}"); }
        public virtual void UnEquip() { }

        public abstract bool ShouldUse();

        public virtual void Enter() { }// Debug.Log($"Entered: {baseData.id}"); }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
