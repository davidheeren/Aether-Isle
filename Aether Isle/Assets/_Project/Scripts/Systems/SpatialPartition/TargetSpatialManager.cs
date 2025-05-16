using Game;
using Utilities;

namespace SpatialPartition
{
    public class TargetSpatialManager : SpatialHashGrid<Target, TargetSpatialManager>
    {
        private void FixedUpdate()
        {
            UpdateEntriesPosition();
        }
    }
}
