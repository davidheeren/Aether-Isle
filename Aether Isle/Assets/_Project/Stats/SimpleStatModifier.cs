using CustomInspector;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(menuName = "Stats/Modifiers/Simple")]
    public class SimpleStatModifier : StatModifier
    {
        [SerializeField] SimpleModifierType modifierType;
        [SerializeField] float a;
        [SerializeField, ShowIf(nameof(IsClamp))] float b;

        // Only display value B if we are clamping
        bool IsClamp() => modifierType == SimpleModifierType.Clamp;


        public override float ModifyStat(float stat)
        {
            switch (modifierType)
            {
                case SimpleModifierType.Add:
                    return stat + a;
                case SimpleModifierType.Multiply:
                    return stat * a;
                case SimpleModifierType.Max:
                    return Mathf.Max(stat, a);
                case SimpleModifierType.Min:
                    return Mathf.Min(stat, a);
                case SimpleModifierType.Clamp:
                    return Mathf.Clamp(stat, a, b);
            }

            Debug.LogError("Something went wrong with SimpleModifier");
            return stat;
        }
        public override float priority
        {
            get
            {
                switch (modifierType)
                {
                    case SimpleModifierType.Add:
                        return 0;
                    case SimpleModifierType.Multiply:
                        return 1;
                    case SimpleModifierType.Max:
                        return 10;
                    case SimpleModifierType.Min:
                        return 10;
                    case SimpleModifierType.Clamp:
                        return 10;
                }

                return 0;
            }
        }

        enum SimpleModifierType
        {
            Add,
            Multiply,
            Max,
            Min,
            Clamp
        }
    }
}
