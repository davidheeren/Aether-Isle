using System;

namespace StateTree
{
    public class EventCondition : Condition
    {
        // This could return true a frame after the event if triggered

        EventSwitch eventSwitch;

        public EventCondition(Action action) : base(null)
        {
<<<<<<< Updated upstream
            action += Trigger;
=======
            eventSwitch = new EventSwitch(ref action);
            return this;
>>>>>>> Stashed changes
        }

        public override bool Calculate()
        {
            return eventSwitch.happened;
        }
    }
}
