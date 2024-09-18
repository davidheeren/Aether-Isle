
namespace StateTree
{
    public class EventCondition : Condition
    {
        EventSwitch eventSwitch;

        public EventCondition(EventSwitch eventSwitch)
        {
            this.eventSwitch = eventSwitch;
        }

        public override bool Calculate()
        {
            return eventSwitch.happened;
        }
    }
}
