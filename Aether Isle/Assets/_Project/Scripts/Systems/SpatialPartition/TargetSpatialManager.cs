using Game;
using UnityEngine;
using System.Collections.Generic;
using Utilities;

namespace SpatialPartition
{
    public class TargetSpatialManager : SpatialHashGrid<Target>
    {
        HashSet<Target> targetsToSwitch = new HashSet<Target>();

        public static TargetSpatialManager _instance;
        public static TargetSpatialManager Instance => Singleton<TargetSpatialManager>.GetInstanceHelper(_instance);

        private void FixedUpdate()
        {
            foreach (Target target in AllEntries)
            {
                if (cellPositions[target] != WorldToCellPosition(target.Position))
                {
                    targetsToSwitch.Add(target);
                }
            }

            foreach (Target target in targetsToSwitch)
            {
                Remove(target);
                Add(target);
            }

            targetsToSwitch.Clear();
        }
    }
}
