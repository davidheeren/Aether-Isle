using UnityEngine;

namespace Game
{
    public class AimGraphic : MonoBehaviour
    {
        [SerializeField] Player player;

        const float turnSpeed = 50;

        private void Update()
        {
            float angle = Mathf.Atan2(player.aimDir.y, player.aimDir.x) * Mathf.Rad2Deg - 90;

            angle = Mathf.DeltaAngle(transform.eulerAngles.z, angle) * turnSpeed * Time.deltaTime + transform.eulerAngles.z;

            transform.eulerAngles = Vector3.forward * angle;
        }
    }
}
