using Game;
using Utilities;

namespace Inventory
{
    public abstract class UseableData : ItemData
    {
        public abstract Useable CreateUseable(ActorComponents components);
    }
}
