using CustomInspector;
using UnityEngine;

namespace Stats
{
    public abstract class StatModifier : ScriptableObject
    {
        [field: SerializeField, TooltipBox("0 means manually removing modifier instead of time")] 
        public float time { get; private set; }
        [field: SerializeField] public StatType statType { get; private set; }

        public abstract float priority { get; }
        public abstract float ModifyStat(float stat);
    }
}
