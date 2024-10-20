using CustomEventSystem;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Health))]
    public class PlayerBroadcast : MonoBehaviour
    {
        [SerializeField] GameEvent onPlayerSpawn;
        [SerializeField] GameEvent onPlayerHealthChange;
        [SerializeField] GameEvent onPlayerDie;

        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Start()
        {
            onPlayerSpawn.Raise(gameObject);
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
            onPlayerHealthChange.Raise(health.currentHealth);
        }

        void OnDie()
        {
            onPlayerDie.Raise();
        }
    }
}
