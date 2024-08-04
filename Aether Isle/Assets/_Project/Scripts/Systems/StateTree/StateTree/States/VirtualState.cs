using System;

namespace StateTree
{
    public class VirtualState : State
    {
        Action EnterMethod;
        Action UpdateMethod;
        Action FixedUpdateMethod;
        Action ExitMethod;

        public VirtualState Create(Action EnterMethod = null, Action UpdateMethod = null, Action FixedUpdateMethod = null, Action ExitMethod = null, Node child = null)
        {
            CreateState(child);

            this.EnterMethod = EnterMethod;
            this.UpdateMethod = UpdateMethod;
            this.FixedUpdateMethod = FixedUpdateMethod;
            this.ExitMethod = ExitMethod;

            return this;
        }

        protected override void EnterState()
        {
            base.EnterState();
            EnterMethod?.Invoke();
        }

        protected override void UpdateState()
        {
            base.UpdateState();
            UpdateMethod?.Invoke();
        }
        protected override void FixedUpdateState()
        {
            base.FixedUpdateState();
            FixedUpdateMethod?.Invoke();
        }
        protected override void ExitState()
        {
            base.ExitState();
            ExitMethod?.Invoke();
        }
    }
}
