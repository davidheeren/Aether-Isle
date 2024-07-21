using CustomInspector;
using Pathfinding;
using System;
using UnityEngine;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [Button(nameof(UpdateGrid))]
        [SerializeField] Collider2D col;

        Bounds bounds;

        private void Awake()
        {
            bounds = col.bounds;
        }

        private void UpdateGrid()
        {
            PathGrid.Instance.UpdateGrid(bounds);

            if (gameObject.activeInHierarchy)
                bounds = col.bounds;
        }
    }
}
