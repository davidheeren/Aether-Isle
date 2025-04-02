using CustomEventSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SegmentBarUI : MonoBehaviour
    {
        // Obsolete Do not use

        [SerializeField] GameObject segmentTemplate;
        [SerializeField] int segmentCount = 8;
        [SerializeField] float initialOffset = -4;
        [SerializeField] float segmentOffset = 25;

        GameObject[] segments;
        int playerMaxHealth;

        void SetupSegments()
        {
            segments = new GameObject[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                segments[i] = Instantiate(segmentTemplate, transform);
                segments[i].transform.localPosition = new Vector3(i * segmentOffset + initialOffset, 0, 0);
                segments[i].name = "Segment " + i;
            }

            UpdateSegments(playerMaxHealth);
        }

        void UpdateSegments(float health)
        {
            float healthPercent = health / playerMaxHealth;

            for (int i = 0; i < segments.Length; i++)
            {
                float segmentPercent = i / (float)segmentCount;

                segments[i].SetActive(segmentPercent < healthPercent);
            }
        }

        public void OnPlayerHealthChange(GameEventData data)
        {
            UpdateSegments(data.GetData<float>());
        }

        public void OnPlayerSpawn(GameEventData data)
        {
            playerMaxHealth = (int)data.GetData<GameObject>().GetComponent<Health>().MaxHealth;
            SetupSegments();
        }
    }
}
