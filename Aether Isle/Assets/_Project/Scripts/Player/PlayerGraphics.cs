using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class PlayerGraphics : MonoBehaviour
    {
        Animator animator;
        
        enum States { idle, walk, attack }
        States state = States.idle;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            Vector2 move = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
            bool isMove = move != Vector2.zero;

            if (isMove && state != States.walk)
            {
                animator.Play("PlayerWalk");
                state = States.walk;
            }

            if (!isMove && state != States.idle)
            {
                animator.Play("PlayerIdle");
                state = States.idle;
            }

            if (move.x != 0)
                transform.localScale = new Vector3(Mathf.Sign(move.x), 1, 1);
        }
    }
}
