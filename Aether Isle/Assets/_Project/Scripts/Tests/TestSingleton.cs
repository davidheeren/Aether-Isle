using Utilities;

namespace Game
{
    public class TestSingleton : Singleton<TestSingleton>
    {
        public float myValue = 5;
    }
}
