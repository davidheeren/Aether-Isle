using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HitShader : MonoBehaviour
    {
        [SerializeField] Health health;
        const float contrastTime = 0.25f;

        Material material;
        Timer timer;

        private void Awake()
        {
            material = GetComponent<SpriteRenderer>().material;
            health.OnDamage += SetContrast;
            health.OnDie += Die;

            timer = new Timer(contrastTime);
            timer.Stop();
        }

        void SetContrast(DamageStats damage, Vector2? dir)
        {
            material.SetFloat("_Contrast", 1);
            timer.Reset();
        }

        void Die()
        {
            material.SetFloat("_Contrast", 0);
            timer.Stop();
        }

        private void Update()
        {
            if (timer.isDone)
            {
                material.SetFloat("_Contrast", 0);
                timer.Stop();
            }
        }
    }
}
