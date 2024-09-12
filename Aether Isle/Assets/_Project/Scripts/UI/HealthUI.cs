using EventSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Sprite segmentSprite;
        [SerializeField] int segmentCount = 8;
        [SerializeField] float initialOffset = -4;
        [SerializeField] float segmentOffset = 25;

        GameObject[] segments;

        void Awake()
        {
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

            UpdateSegments(segmentCount);
        }

        void UpdateSegments(int health)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (i < health)
                    segments[i].SetActive(true);
                else
                    segments[i].SetActive(false);
            }
        }

        public void OnPlayerHealthChange(GameEventData data)
        {
            UpdateSegments(data.GetData<int>());
        }
    }
}
