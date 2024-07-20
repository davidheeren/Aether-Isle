using UnityEngine;
using Utilities;

namespace Game
{
    public class CameraMovement : Singleton<CameraMovement>
    {
        [SerializeField] Transform target;
        [SerializeField] bool hardLock = false;
        [SerializeField] float decay = 10;

        private void Awake()
        {
            if (target == null)
                target = GameObject.FindGameObjectWithTag("Player").transform;

            if (target != null)
                SetPos(target.position);
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

            SetPos(pos);
        }

        void SetPos(Vector2 pos)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
