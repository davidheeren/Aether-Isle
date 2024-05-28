using System;
using UnityEngine;

namespace StateTree
{
    [Serializable]
    public class HolderState : State
    {
        // This state does nothing
        // This would be useful if you had two states that you needed to lock with a modifier
        // Then you could have this state be the one that locks then have your selector as a child

        public HolderState(Node child) : base(null, child) { }
    }
}
