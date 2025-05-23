using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Stats
{
    public class ActorStats : MonoBehaviour
    {
        [SerializeField] BaseStats baseStats;
        [SerializeField] List<StatModifier> initialModifiers;

        Dictionary<StatType, List<TimedModifier>> modifiers = new();

        private void Awake()
        {
            foreach (StatModifier initial in initialModifiers)
            {
                AddModifier(initial);
            }
        }

        public void AddModifier(StatModifier modifier)
        {
            if (modifier == null) { Debug.LogError("Modifier is null"); return; }

            if (modifiers.TryGetValue(modifier.statType, out var list))
            {
                list.Add(new TimedModifier(modifier));
                OrderModifiersByPriority(ref list);
            }
            else
            {
                modifiers[modifier.statType] = new List<TimedModifier> { new TimedModifier(modifier) };
            }
        }

        // Only removes one modifier if there are duplicates
        public void RemoveModifier(StatModifier modifier)
        {
            if (modifier == null) { Debug.LogError("Modifier is null"); return; }

            if (modifiers.TryGetValue(modifier.statType, out var list))
            {
                foreach (TimedModifier timedModifier in list)
                {
                    if (timedModifier.modifier == modifier)
                    {
                        list.Remove(timedModifier);
                        break;
                    }
                }

                OrderModifiersByPriority(ref list);
            }
        }

        private void OrderModifiersByPriority(ref List<TimedModifier> list)
        {
            // Sort list so that highest priorities modify first
            list.Sort((x, y) => y.modifier.priority.CompareTo(x.modifier.priority));
        }

        public float GetStat(StatType type)
        {
            if (baseStats == null) { Debug.LogError("Base stats is null"); return 0; }

            if (baseStats.stats.TryGetValue(type, out float baseStat))
            {
                return GetStat(type, baseStat);
            }

            Debug.LogError("Base stats does not contain typeof: " + type.ToString());
            return 0;
        }

        public float GetStat(StatType type, float baseStat)
        {
            float modifiedStat = baseStat;

            if (modifiers.TryGetValue(type, out List<TimedModifier> list))
            {
                // Remove all modifiers whose timers are done
                list.RemoveAll(timedModifier => timedModifier.ShouldRemove());

                foreach (TimedModifier timedModifier in list)
                {
                    modifiedStat = timedModifier.modifier.ModifyStat(baseStat, modifiedStat);
                }

                return modifiedStat;
            }
            else
            {
                return modifiedStat;
            }
        }

        public class TimedModifier
        {
            public StatModifier modifier { get; private set; }
            Timer timer;

            public TimedModifier(StatModifier modifier)
            {
                this.modifier = modifier;

                if (modifier.time > 0) 
                    timer = new Timer(modifier.time);
            }

            public bool ShouldRemove()
            {
                if (timer == null) return false;

                return timer.IsDone;
            }
        }
    }
}
