
namespace StateTree
{
    public class LockNullModifier : Modifier
    {
        // DO NOT USE YET
        // Combination of Null Duration, Lock Duration, and Null Cooldown

        float? duration;
        int? lockDepth;
        bool canReenter;
        float? cooldown;

        SimpleTimer durationTimer;
        SimpleTimer cooldownTimer;

        public LockNullModifier(float? duration, int? lockDepth, float? cooldown, Node child) : this(duration, lockDepth, false, cooldown, child) { }
        public LockNullModifier(float? duration, int? lockDepth, bool canReenter, float? cooldown, Node child) : base(null, child)
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
                cooldownTimer.Stop();
            }
        }

        protected override void Setup()
        {
            base.Setup();
            firstChildState.canReenter = canReenter;
        }

        protected override void EnterChildState()
        {

        }

        protected override void ExitChildState()
        {

        }

        public override State Evaluate()
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateChildState()
        {
            
        }
    }
}
