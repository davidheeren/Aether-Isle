using StateTree;
using UnityEngine;
using Utilities;

namespace Game
{
    [RequireComponent(typeof(Movement))]
    public class Player : MonoBehaviour
    {
        [SerializeField] float rollSpeed = 10;
        [SerializeField] GameObject attackPrefab;
        [SerializeField] AudioClip rollSFX;
        [SerializeField] AudioClip attackSFX;

        Movement movement;
        PlayerAimDirection aim;

        [SerializeField] RootState rootState;

        private void Awake()
        {
            movement = GetComponent<Movement>();
            aim = GetComponent<PlayerAimDirection>();

            Node runBranch = new VirtualState(null, UpdateRunState, null, null);
            Node rollBranch = new NullBeforeModifier(0.5f, new LockModifier(0.25f, new VirtualState(EnterRollState, UpdateRollState, null, null)));
            Node attackBranch = new NullBeforeModifier(0.5f, new LockModifier(0.25f, new VirtualState(EnterAttackState, null, null, null)));



            rootState = new RootState(rootState.CopyJson(), new Selector(new Node[] { new If(new VirtualCondition(RollCondition), rollBranch), new If(new VirtualCondition(AttackCondition), attackBranch), runBranch }));
        }

        private void Update()
        {
            rootState.UpdateStateTree();
        }

        void UpdateRunState()
        {
            movement.MoveVelocity(InputManager.Instance.input.Game.Move.ReadValue<Vector2>());
        }

        void EnterAttackState()
        {
            Instantiate(attackPrefab, transform.position + (Vector3)aim.aimDir * 0.75f, Quaternion.Euler(0, 0, Mathf.Atan2(aim.aimDir.y, aim.aimDir.x) * Mathf.Rad2Deg));
            SFXManager.Instance.PlaySFXClip(attackSFX, transform.position);
        }

        Vector2 rollDir;

        void EnterRollState()
        {
            rollDir = InputManager.Instance.input.Game.Move.ReadValue<Vector2>();
            SFXManager.Instance.PlaySFXClip(rollSFX, transform.position);
        }

        void UpdateRollState()
        {
            movement.MoveVelocity(rollDir, speed: rollSpeed);
        }

        bool RollCondition()
        {
            return InputManager.Instance.input.Game.Roll.WasPressedThisFrame() && InputManager.Instance.input.Game.Move.ReadValue<Vector2>() != Vector2.zero;
        }

        bool AttackCondition()
        {
            return InputManager.Instance.input.Game.Attack.WasPressedThisFrame();
        }
    }
}
