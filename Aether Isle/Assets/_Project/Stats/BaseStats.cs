using CustomInspector;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(menuName = "Stats/BaseStats")]
    public class BaseStats : ScriptableObject
    {
        [field: SerializeField, Dictionary] public SerializableDictionary<StatType, float> stats { get; private set; } = new();
    }
}
