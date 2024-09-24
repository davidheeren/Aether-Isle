using CustomInspector;
using Stats;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ObjectStats))]
    public class StatsTest : MonoBehaviour
    {
        [Button(nameof(AddModifier))]
        [Button(nameof(RemoveModifier))]
        [Button(nameof(PrintStat))]
        [SerializeField] StatModifier modifier;
        [SerializeField] bool alwaysPrint;
        ObjectStats stats;

        void Awake()
        {
            stats = GetComponent<ObjectStats>();
        }

        private void Update()
        {
            if (alwaysPrint)
                PrintStat();
        }

        void AddModifier()
        {
            stats.AddModifier(modifier);
        }

        void RemoveModifier()
        {
            stats.RemoveModifier(modifier);
        }

        void PrintStat()
        {
            print(stats.GetStat(modifier.statType));
        }
    }
}
