using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Sprite segmentSprite;
        [SerializeField] int segmentCount = 8;
        [SerializeField] float initialOffset = 150;
        [SerializeField] float segmentOffset = 100;

        GameObject[] segments;

        Health playerHealth;

        void Start()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Health>();

            segments = new GameObject[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                GameObject obj = new GameObject("Segment " + i);
                obj.transform.parent = transform;
                obj.transform.localPosition = new Vector3(i * segmentOffset + initialOffset, 0, 0);

                segments[i] = obj;

                Image img = obj.AddComponent<Image>();
                img.sprite = segmentSprite;
                img.SetNativeSize();
            }

            UpdateSegments();
            playerHealth.OnDamage += UpdateSegments;
        }

        private void UpdateSegments(DamageStats damage, Vector2? dir)
        {
            // Wrapper
            UpdateSegments();
        }

        void UpdateSegments()
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (i < playerHealth.currentHealth)
                    segments[i].SetActive(true);
                else
                    segments[i].SetActive(false);
            }
        }
    }
}
