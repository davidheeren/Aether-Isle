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
            bool isMove = InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;

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
        }
    }
}
