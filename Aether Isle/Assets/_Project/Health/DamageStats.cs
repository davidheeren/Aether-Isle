using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Damage")]
    public class DamageStats : ScriptableObject
    {
        public int damage = 1;
        public float knockbackSpeed = 10;
        public float stunTime = 0.5f;
    }
}
