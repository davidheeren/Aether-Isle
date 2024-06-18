using UnityEngine;
using StateTree;
using System;

namespace Game
{
    [Serializable]
    public class CheckGroundCondition : Condition
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] Vector2 boxOffset = new Vector2(0, -0.35f);
        [SerializeField] Vector2 boxSize = new Vector2(0.4f, 0.2f);

        Transform transform;

        private CheckGroundCondition() : base(null) { }
        public CheckGroundCondition(string copyJson, Transform transform) : base(copyJson)
        {
            this.transform = transform;
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
