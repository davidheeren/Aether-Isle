using UnityEngine;
using Utilities;

namespace Game
{
    public class AimGraphic : MonoBehaviour
    {
        [SerializeField] PlayerAimDirection aim;

        [SerializeField] float decay = 50;

        private void Update()
        {
            float targetAngle = Mathf.Atan2(aim.aimDir.y, aim.aimDir.x) * Mathf.Rad2Deg - 90;

            transform.eulerAngles = Vector3.forward * Smoothing.ExpDecayAngle(transform.eulerAngles.z, targetAngle, decay, Time.deltaTime);
        }
    }
}
