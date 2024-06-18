using UnityEngine;
using StateTree;
using System;
using Utilities;

namespace Game
{
    [Serializable]
    public class ZombieChaseState : State
    {
        [SerializeField] float chaseSpeed = 3;
        [SerializeField] float evaluateDirTime = 0.25f;

        Vector2 dir;

        Ref<Transform> target = new Ref<Transform>();
        Transform transform;
        Movement movement;
        Animator animator;
        Timer timer;

        private ZombieChaseState() : base(null, null) { }
        public ZombieChaseState(string copyJson, Ref<Transform> target, Transform transform, Movement movement, Animator animator, Node child = null) : base(copyJson, child)
        {
            this.target = target;
            this.transform = transform;
            this.movement = movement;
            this.animator = animator;

            timer = new Timer(evaluateDirTime);
        }

        protected override void EnterState()
        {
            base.EnterState();

            animator.Play("Run");

            SetDir();
            timer.Reset();
        }

        protected override void UpdateState()
        {
            if (timer.isDone)
            {
                SetDir();
                timer.Reset();
            }

            movement.Move(dir * chaseSpeed);
        }

        void SetDir()
        {
            dir = (target.value.position - transform.position).normalized;
        }
    }
} 
