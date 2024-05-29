using UnityEngine;

namespace Game
{
    public class FlipFromMovement : MonoBehaviour
    {
        [SerializeField] Movement movement;

        void Update()
        {
            if (movement.targetVelocity.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(movement.targetVelocity.x), 1, 1);
            }
        }
    }
}
