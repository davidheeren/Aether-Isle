using EventSystem;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Health))]
    public class PlayerHealthExtension : MonoBehaviour
    {
        [SerializeField] GameEvent onPlayerHealthChange;
        [SerializeField] GameEvent onPlayerDie;

        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnDamage += OnDamage;
            health.OnDie += OnDie;
        }

        private void OnDisable()
        {
            health.OnDamage -= OnDamage;
            health.OnDie -= OnDie;
        }

        void OnDamage()
        {
            onPlayerHealthChange.Raise(new GameEventData(health.currentHealth));
        }

        void OnDie()
        {
            onPlayerDie.Raise();
        }
    }
}
