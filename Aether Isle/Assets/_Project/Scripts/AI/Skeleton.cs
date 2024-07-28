using UnityEngine;

namespace Game
{
    public class Skeleton : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float distance = 10;
        [SerializeField] float speed = 5;
        [SerializeField] float orbitMultiplier = 5;

        [SerializeField] float angleDir = 1;

        void Update()
        {
            Vector2 targetDir = (target.position - transform.position).normalized;

            Vector2 orbitDir = Vector3.Cross(Vector3.forward * angleDir, targetDir);
            Debug.DrawRay(transform.position, orbitDir * orbitMultiplier, Color.red);

            Vector2 moveAway = (-targetDir * distance + (Vector2)target.position) - (Vector2)transform.position;
            Debug.DrawRay(transform.position, moveAway, Color.blue);

            Vector2 moveDir = (moveAway + orbitDir * orbitMultiplier).normalized;
            transform.position += (Vector3)moveDir * Time.deltaTime * speed;
            Debug.DrawRay(transform.position, moveDir * 2, Color.green);
        }
    }
}
