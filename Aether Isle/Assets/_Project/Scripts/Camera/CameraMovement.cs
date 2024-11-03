using CustomEventSystem;
using UnityEngine;
using Utilities;

namespace Game
{
    public class CameraMovement : Singleton<CameraMovement>
    {
        [SerializeField] Transform target;
        [SerializeField] bool hardLock = false;
        [SerializeField] float decay = 10;
        [SerializeField] bool useFreeLook;
        [SerializeField] float freeLookSpeed = 10;

        public void OnPlayerSpawn(GameEventData data)
        {
            if (target == null)
                target = data.GetData<GameObject>().transform;

            if (target != null)
                SetPos(target.position);
        }

        void LateUpdate()
        {
            if (target != null)
            {
                UpdatePositionFromTarget();
                return;
            }

            if (useFreeLook)
                UpdatePositionUsingFreeLook();
        }

        void UpdatePositionFromTarget()
        {
            Vector2 pos = target.position;

            if (!hardLock)
            {
                pos = Smoothing.ExpDecay(transform.position, pos, decay, Time.deltaTime);
            }

            SetPos(pos);
        }

        void UpdatePositionUsingFreeLook()
        {
            transform.position += (Vector3)InputManager.Instance.input.Game.Move.ReadValue<Vector2>() * freeLookSpeed * Time.deltaTime;
        }

        void SetPos(Vector2 pos)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
