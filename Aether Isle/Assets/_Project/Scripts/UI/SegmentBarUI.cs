using EventSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SegmentBarUI : MonoBehaviour
    {
        [SerializeField] Sprite segmentSprite;
        [SerializeField] int segmentCount = 8;
        [SerializeField] float initialOffset = -4;
        [SerializeField] float segmentOffset = 25;

        GameObject[] segments;

        void Start()
        {
            SetupSegments();
        }

        void SetupSegments()
        {
            segments = new GameObject[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                GameObject obj = new GameObject("Segment " + i);
                obj.transform.parent = transform;
                obj.layer = LayerMask.NameToLayer("UI");

                segments[i] = obj;

                Image img = obj.AddComponent<Image>();
                img.sprite = segmentSprite;
                
                // Having issues with the size. Doing this in Awake seems to have different results. Also size is 0.44 even when not native size
                img.SetNativeSize();
                obj.transform.localScale = Vector3.one;
                img.rectTransform.anchorMax = Vector2.up * 0.5f;
                img.rectTransform.anchorMin = Vector2.up * 0.5f;
                img.rectTransform.pivot = Vector2.up * 0.5f;

                // Set position
                obj.transform.localPosition = new Vector3(i * segmentOffset + initialOffset, 0, 0);
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
