using CustomEventSystem;
using System;
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
            health.OnHealthAdd += OnHealthAdd;
            health.OnDie += OnDie;
        }

        private void OnDisable()
        {
            health.OnDamage -= OnDamage;
            health.OnHealthAdd -= OnHealthAdd;
            health.OnDie -= OnDie;
        }

        void OnDamage(float damage, float stunTime, ActorComponents source)
        {
            onPlayerHealthChange.Raise(health.CurrentHealth);
        }

        private void OnHealthAdd(float add)
        {
            onPlayerHealthChange.Raise(health.CurrentHealth);
        }

        void OnDie()
        {
            onPlayerDie.Raise();
        }
    }
}
