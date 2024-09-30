using UnityEngine;
using Utilities;

namespace Game
{
    public class FlipFromMovement : MonoBehaviour
    {
        [SerializeField] Movement movement;

        Timer timer;

        private void Awake()
        {
            timer = new Timer(0.1f);
        }

        void Update()
        {
            if (movement.targetVelocity.x != 0 && timer.isDone)
            {
                transform.localScale = new Vector3(Mathf.Sign(movement.targetVelocity.x), 1, 1);
                timer.Reset();
            }
        }
    }
}
