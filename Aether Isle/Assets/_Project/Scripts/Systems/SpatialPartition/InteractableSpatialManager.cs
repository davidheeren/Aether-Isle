using Game;
using Utilities;

namespace SpatialPartition
{
    public class InteractableSpatialManager : SpatialHashGrid<IInteractable, InteractableSpatialManager>
    {
        private void FixedUpdate()
        {
            UpdateEntriesPosition();
        }
    }
}
