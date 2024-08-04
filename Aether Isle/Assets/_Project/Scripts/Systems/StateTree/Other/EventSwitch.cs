using System;
using UnityEngine;

namespace StateTree
{
    public class EventSwitch
    {
        bool happenSwitch;

        public bool happened
        {
            get
            {
                bool _happened = happenSwitch;
                happenSwitch = false;
                return _happened;
            }
        }

        public EventSwitch(ref Action action)
        {
            action += OnEvent;
        }

        void OnEvent()
        {
            happenSwitch = true;
        }
    }
}
