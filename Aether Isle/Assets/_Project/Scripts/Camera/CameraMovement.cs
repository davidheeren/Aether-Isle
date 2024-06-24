using UnityEngine;
using Utilities;

namespace Game
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] bool hardLock = false;
        [SerializeField] float decay = 10;

        private void Update()
        {
            if (target == null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void LateUpdate()
        {
            if (target == null)
                return;

            Vector2 pos = target.position;

            if (!hardLock)
            {
                pos = Smoothing.ExpDecay(transform.position, pos, decay, Time.deltaTime);
            }

            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
