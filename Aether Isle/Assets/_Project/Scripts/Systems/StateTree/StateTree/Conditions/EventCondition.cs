using System;

namespace StateTree
{
    public class EventCondition : Condition
    {
        //EventSwitch eventSwitch;

        //public EventCondition(EventSwitch eventSwitch, Node child = null) : base(child)
        //{
        //    this.eventSwitch = eventSwitch;
        //}

        // Do not use this class right now
        private EventCondition() : base(null) { }

        public override bool Calculate()
        {
            //return eventSwitch.Happened;
            throw new NotImplementedException();
        }

        protected override void Destroy()
        {
            base.Destroy();

            // Dispose of event switch
        }
    }
}
