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
            health.OnDie += OnDie;

            timer = new Timer(contrastTime);
            timer.Stop();
        }

        private void OnDie()
        {
            material.SetFloat("_Die", 1);
        }

        void SetContrast()
        {
            material.SetFloat("_Contrast", 1);
            timer.Reset();
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
