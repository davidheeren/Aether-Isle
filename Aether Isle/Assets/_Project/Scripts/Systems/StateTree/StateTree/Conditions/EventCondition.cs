using System;

namespace StateTree
{
    public class EventCondition : Condition
    {
        // This could return true a frame after the event if triggered

        bool wasTriggered;

        public EventCondition(Action action)
        {
            action += Trigger;
        }

        public override bool Calculate()
        {
            bool trigger = wasTriggered;
            wasTriggered = false;

            return trigger;
        }

        void Trigger()
        {
            wasTriggered = true;
        }
    }
}
