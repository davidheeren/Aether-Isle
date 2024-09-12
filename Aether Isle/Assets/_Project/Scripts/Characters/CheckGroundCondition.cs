using StateTree;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CheckGroundCondition : Condition
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] Vector2 boxOffset = new Vector2(0, -0.35f);
        [SerializeField] Vector2 boxSize = new Vector2(0.4f, 0.2f);

        Transform transform;

        public CheckGroundCondition Init(Transform transform)
        {
            this.transform = transform;

            return this;
        }

        public override bool Calculate()
        {
            return Physics2D.OverlapBox((Vector2)transform.position + boxOffset, boxSize, 0, layerMask);
        }

        public void DrawBox(Vector2 pos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(pos + boxOffset, boxSize);
        }
    }
}
