using CustomInspector;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Game
{
    [ExecuteInEditMode]
    public class WaypointsContainer : MonoBehaviour
    {
        [Button(nameof(Reverse))]
        [Button(nameof(ToggleWaypointDisplayers))]

        [SerializeField] bool drawLines = true;
        [SerializeField] Sprite waypointSprite;
        [SerializeField] float scale = 2;
        [SerializeField] Color startColor = Color.green;
        [SerializeField] Color defaultColor = Color.blue;

        GameObject[] waypointGOs = new GameObject[0];

        private void OnTransformChildrenChanged() => UpdateChildren();
        private void OnValidate() => UpdateChildren();

        void UpdateChildren()
        {
            if (Application.isPlaying) return;

            waypointGOs = GetChildren();
            for (int i = 0; i < waypointGOs.Length; i++)
            {
                waypointGOs[i].name = "Waypoint " + i;
                waypointGOs[i].transform.localScale = Vector3.one * scale;

                var sr = waypointGOs[i].GetOrAddComponent<SpriteRenderer>();
                sr.sprite = waypointSprite;
                sr.color = i == 0 ? startColor : defaultColor;
                sr.sortingOrder = 200;
            }
        }

        void Awake()
        {
            waypointGOs = GetChildren();
            if (!Application.isPlaying) return;
            EnableWaypointDisplayers(false);
        }

        void EnableWaypointDisplayers(bool enable)
        {
            foreach (GameObject go in waypointGOs)
            {
                go.SetActive(enable);
            }
        }

        void ToggleWaypointDisplayers()
        {
            foreach (var go in waypointGOs)
            {
                go.SetActive(!go.activeSelf);
            }
        }

        GameObject[] GetChildren()
        {
            var output = new GameObject[transform.childCount];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = transform.GetChild(i).gameObject;
            }
            return output;
        }

        private void OnDrawGizmos()
        {
            if (!drawLines) return;

            Vector2[] waypoints = GetWaypoints();
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Color color = i == 0 ? startColor : defaultColor;

                Debug.DrawLine(waypoints[i], waypoints[i + 1], color);
            }

            if (waypoints.Length > 0)
            {
                Vector2 lastWaypoint = waypoints[waypoints.Length - 1];
                Debug.DrawLine(lastWaypoint, waypoints[0], defaultColor);
            }
        }

        void Reverse()
        {
            for (int i = waypointGOs.Length - 1; i > 0; i--)
            {
                waypointGOs[i].transform.SetSiblingIndex(i - waypointGOs.Length);
            }
        }

        // TODO
        void ShiftLeft() { }
        void ShiftRight() { }

        public Vector2[] GetWaypoints()
        {
            return GetChildren().Select(go => (Vector2)go.transform.position).ToArray();
        }
    }
}
