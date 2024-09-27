using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Data/Damage")]
    public class DamageData : ScriptableObject
    {
        public int damage = 1;
        public float knockbackSpeed = 10;
        public float stunTime = 0.5f;
    }
}
