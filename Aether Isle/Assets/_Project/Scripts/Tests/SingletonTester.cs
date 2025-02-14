using UnityEngine;
using Utilities;

namespace Game
{
    public class SingletonTester : MonoBehaviour
    {
        [SerializeField] bool awake = true;
        [SerializeField] bool onDisable;
        [SerializeField] bool onDestroy;

        private void Awake()
        {
            if (!awake)
                return;
            float f = TestSingleton.Instance.myValue;
        }

        private void OnDisable()
        {
            if (!onDisable)
                return;
            print(TestSingleton.RawInstance);
            float f = TestSingleton.Instance.myValue;
        }

        private void OnDestroy()
        {
            if (!onDestroy)
                return;
            print(TestSingleton.RawInstance);
            float f = TestSingleton.Instance.myValue;
        }
    }
}
