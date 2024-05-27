using UnityEngine;

namespace Game
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float smoothAmount = 0;

        void LateUpdate()
        {
            if (target == null)
                return;

            Vector2 pos = target.position;
            
            if (smoothAmount != 0)
            {
                pos = ((target.position - transform.position) / smoothAmount * Time.deltaTime) + transform.position;
            }

            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
