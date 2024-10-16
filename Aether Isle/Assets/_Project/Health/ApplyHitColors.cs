using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ApplyHitColors : MonoBehaviour
    {
        [SerializeField] Health health;

        const float hitTime = 0.25f;

        SpriteRenderer sr;
        Timer timer;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();

            health.OnDamage += OnHit;
            health.OnDie += OnDie;

            timer = new Timer(hitTime);
            timer.Stop();
        }

        private void OnDie()
        {
            sr.SetPropertyFloat("_DieFactor", 1);
            timer.Reset();
        }

        void OnHit()
        {
            sr.SetPropertyFloat("_HitFactor", 1);
            timer.Reset();
        }

        private void Update()
        {
            if (timer.IsDone)
            {
                sr.SetPropertyFloat("_HitFactor", 0);
                sr.SetPropertyFloat("_DieFactor", 0);
                timer.Stop();
            }
        }
    }
}
