using System;

namespace StateTree
{
    public class EventSwitch
    {
        private bool happenSwitch;

        public bool happened
        {
            get
            {
                bool _happened = happenSwitch;
                happenSwitch = false;
                return _happened;
            }
        }

        /// <summary>
        /// Syntax for a lambda expression: (action) => myEvent += action
        /// </summary>
        public EventSwitch(Action<Action> eventSubscribe)
        {
            eventSubscribe(OnEvent);
        }

        public EventSwitch(ref Action action)
        {
            action += OnEvent;
        }

        /// <summary>
        /// Syntax for a lambda expression: (action) => myEvent -= action
        /// </summary>
        public void Dispose(Action<Action> eventSubscribe)
        {
            eventSubscribe(OnEvent);
        }

        public void Dispose(ref Action action)
        {
            action -= OnEvent;
        }

        public void OnEvent()
        {
            happenSwitch = true;
        }
    }
}
