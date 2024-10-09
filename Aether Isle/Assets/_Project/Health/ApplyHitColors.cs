using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ApplyHitColors : MonoBehaviour
    {
        [SerializeField] Health health;

        const float contrastTime = 0.25f;

        SpriteRenderer sr;
        Timer timer;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();

            health.OnDamage += OnHit;
            health.OnDie += OnDie;

            timer = new Timer(contrastTime);
            timer.Stop();
        }

        private void OnDie()
        {
            SetPropertyFloat("_DieFactor", 1);
            timer.Reset();
        }

        void OnHit()
        {
            SetPropertyFloat("_HitFactor", 1);
            timer.Reset();
        }

        private void Update()
        {
            if (timer.IsDone)
            {
                SetPropertyFloat("_HitFactor", 0);
                SetPropertyFloat("_DieFactor", 0);
                timer.Stop();
            }
        }

        // More performant that material.SetFloat() because it does not create a new instance
        void SetPropertyFloat(string property, float value)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            sr.GetPropertyBlock(block, 0);
            block.SetFloat(property, value);
            sr.SetPropertyBlock(block, 0);
        }
    }
}
