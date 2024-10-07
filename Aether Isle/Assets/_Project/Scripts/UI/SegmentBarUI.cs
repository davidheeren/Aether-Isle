using CustomEventSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SegmentBarUI : MonoBehaviour
    {
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

        void UpdateSegments(int health)
        {
            float healthPercent = health / (float)playerMaxHealth;

            for (int i = 0; i < segments.Length; i++)
            {
                float segmentPercent = i / (float)segmentCount;

                segments[i].SetActive(segmentPercent < healthPercent);
            }
        }

        public void OnPlayerHealthChange(GameEventData data)
        {
            UpdateSegments(data.GetData<int>());
        }

        public void OnPlayerSpawn(GameEventData data)
        {
            playerMaxHealth = data.GetData<GameObject>().GetComponent<Health>().maxHealth;
            SetupSegments();
        }
    }
}
