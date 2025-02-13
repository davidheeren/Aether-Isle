using UnityEngine;

namespace Game
{
    public class SwingManager : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sr;

        float angleOffset;

        public SwingManager Init(Sprite sprite, float angleOffset = -45)
        {
            sr.sprite = sprite;
            this.angleOffset = angleOffset;
            return this;
        }

        public SwingManager SetAngle(float angle)
        {
            transform.eulerAngles = Vector3.forward * (angle + angleOffset);
            return this;
        }
    }
}
