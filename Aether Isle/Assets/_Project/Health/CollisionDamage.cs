using CustomInspector;
using DamageSystem;
using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class CollisionDamage : MonoBehaviour
    {
        [SerializeField] LayerMask damageMask;
        [SerializeField] DamageData damageData;

        [TooltipBox("This will use the rotation of this instead of the dir of the collision")]
        [SerializeField] DamageApplier.DamageDirection damageDirection;

        ActorComponents source;

        DamageApplier applier;

        private void Awake()
        {
            source = GetComponent<ActorComponents>();

            if (damageData == null)
                Debug.LogError("Damage Data is null", gameObject);

            applier = new DamageApplier(damageMask, damageData, damageDirection, source, transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            applier.Damage(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            applier.Damage(collision.collider);
        }
    }
}
