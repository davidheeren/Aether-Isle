namespace StateTree
{
    public class LockNullModifier : Modifier
    {
        // Combination of Null Duration, Lock Duration, and Null Cooldown

        // First in the update it ends the lock

        float? duration;
        int? lockDepth;
        bool? canReenter;
        float? cooldown;

        bool isLocked;
        bool hasEnteredSateOnce; // If the delay is infinite, we can still enter the state once


        SimpleTimer durationTimer;
        SimpleTimer cooldownTimer;

        public LockNullModifier(float? duration, int? lockDepth, float? cooldown, Node child) : this(duration, lockDepth, null, cooldown, child) { }
        public LockNullModifier(float? duration, int? lockDepth, bool? canReenter, float? cooldown, Node child) : base(null, child)
        {
            this.duration = duration;
            this.lockDepth = lockDepth;
            this.canReenter = canReenter;
            this.cooldown = cooldown;

            if (duration != null)
            {
                durationTimer = new SimpleTimer(duration.Value);
                durationTimer.Stop();
            }

            if (cooldown != null)
            {
                cooldownTimer = new SimpleTimer(cooldown.Value);
                cooldownTimer.ForceDone();
            }
        }

        protected override void Setup()
        {
            base.Setup();

            if (canReenter != null)
                subState.canReenter = canReenter.Value;
        }

        protected override void EnterSubState()
        {
            // Duration
            if (durationTimer != null)
                durationTimer.Reset();

            subState.LockSuperStates(lockDepth, true);
            isLocked = true;

            // Cooldown
            if (cooldownTimer != null)
                cooldownTimer.ForceDone();
        }

        protected override void ExitSubState()
        {
            Exit();
        }

        public override State Evaluate()
        {
            // Cooldown
            if (cooldownTimer != null)
            {
                if (!cooldownTimer.isDone)
                    return null;
            }
            else if (hasEnteredSateOnce)
                return null;
            hasEnteredSateOnce = true;

            // Default
            return child.Evaluate();
        }

        protected override void UpdateSubState()
        {
            if (durationTimer != null)
                if (durationTimer.isDone)
                    Exit();
        }

        void Exit()
        {
            if (isLocked)
            {
                subState.LockSuperStates(lockDepth, false);
                isLocked = false;
            }

            if (durationTimer != null)
                durationTimer.Stop();

            if (cooldownTimer != null)
                cooldownTimer.Reset();
        }
    }
}
