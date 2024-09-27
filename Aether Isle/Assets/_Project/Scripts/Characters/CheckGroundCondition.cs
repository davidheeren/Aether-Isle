using StateTree;
using UnityEngine;

namespace Game
{
    public class CheckGroundCondition : Condition
    {
        Data data;
        Transform transform;

        public CheckGroundCondition(Data data, Transform transform, Node child = null) : base(child)
        {
            this.data = data;
            this.transform = transform;
        }

        [System.Serializable]
        public class Data
        {
            public LayerMask layerMask;
            public Vector2 boxOffset = new Vector2(0, -0.35f);
            public Vector2 boxSize = new Vector2(0.4f, 0.2f);
        }

        public override bool Calculate()
        {
            return Physics2D.OverlapBox((Vector2)transform.position + data.boxOffset, data.boxSize, 0, data.layerMask);
        }

        public static void DrawBox(Data data, Vector2 pos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(pos + data.boxOffset, data.boxSize);
        }
    }
}
