using CustomInspector;
using System;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(menuName = "Stats/Modifiers/Simple")]
    public class SimpleStatModifier : StatModifier
    {
        [SerializeField] ModifierType modifierType;
        [SerializeField] float a;
        [SerializeField, ShowIf(nameof(IsClamp))] float b;

        // Only display value B if we are clamping
        bool IsClamp() => modifierType == ModifierType.Clamp;


        public override float ModifyStat(float baseStat, float modifiedStat)
        {
            switch (modifierType)
            {
                case ModifierType.Add:
                    return modifiedStat + a;
                case ModifierType.AddPercent:
                    return modifiedStat + a / 100 * baseStat;
                case ModifierType.Multiply:
                    return modifiedStat * a;
                case ModifierType.Max:
                    return Mathf.Max(modifiedStat, a);
                case ModifierType.Min:
                    return Mathf.Min(modifiedStat, a);
                case ModifierType.Clamp:
                    return Mathf.Clamp(modifiedStat, Mathf.Min(a, b), Mathf.Max(a, b));
            }

            Debug.LogError("Something went wrong with SimpleModifier");
            return modifiedStat;
        }
        public override float priority
        {
            get
            {
                switch (modifierType)
                {
                    case ModifierType.Add:
                        return 0;
                    case ModifierType.AddPercent:
                        return 0;
                    case ModifierType.Multiply:
                        return 5;
                    case ModifierType.Max:
                        return 10;
                    case ModifierType.Min:
                        return 10;
                    case ModifierType.Clamp:
                        return 10;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        enum ModifierType
        {
            Add,
            AddPercent,
            Multiply,
            Max,
            Min,
            Clamp
        }
    }
}
