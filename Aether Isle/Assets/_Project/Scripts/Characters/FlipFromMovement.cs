using UnityEngine;
using Utilities;

namespace Game
{
    public class FlipFromMovement : MonoBehaviour
    {
        [SerializeField] Movement movement;

        Timer timer;
        //SpriteRenderer[] renderers;

        private void Awake()
        {
            timer = new Timer(0.1f);
            //renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        void Update()
        {
            if (movement.targetVelocity.x != 0 && timer.isDone)
            {
                transform.localScale = new Vector3(Mathf.Sign(movement.targetVelocity.x), 1, 1);

                //bool flip = movement.targetVelocity.x < 0;

                //foreach (SpriteRenderer renderer in renderers)
                //{
                //    renderer.flipX = flip;
                //}

                timer.Reset();
            }
        }
    }
}
