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
        [SerializeField, HideInInspector] GameObject[] waypointGOs = new GameObject[0];
        [SerializeField] bool drawLines = true;

        [Button(nameof(UpdateChildrenManual))]
        [Button(nameof(Reverse))]
        [SerializeField] Sprite waypointSprite;
        [SerializeField] float scale = 2;
        [SerializeField] Color startColor = Color.green;
        [SerializeField] Color defaultColor = Color.blue;

        const float circleRadius = 0.25f;

        void UpdateChildrenManual() => OnTransformChildrenChanged();

        private void OnTransformChildrenChanged()
        {
            waypointGOs = GetChildren();
            for (int i = 0; i < waypointGOs.Length; i++)
            {
                waypointGOs[i] = transform.GetChild(i).gameObject;
                waypointGOs[i].name = "Waypoint " + i;
                waypointGOs[i].transform.localScale = Vector3.one * scale;
                waypointGOs[i].SetActive(true);

                var sr = waypointGOs[i].GetOrAddComponent<SpriteRenderer>();
                sr.sprite = waypointSprite;
                sr.color = i == 0 ? startColor : defaultColor;
                sr.sortingOrder = 200;
            }

        }

        void Awake()
        {
            if (!Application.isPlaying) return;

            foreach (var go in waypointGOs)
            {
                go.SetActive(false);
            }

            //drawLines = false;
        }

        GameObject[] GetChildren()
        {
            var output = new GameObject[transform.childCount];
            for (int i = 0; i < waypointGOs.Length; i++)
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
            return waypointGOs.Select(go => (Vector2)go.transform.position).ToArray();
        }
    }
}
