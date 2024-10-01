using UnityEngine;
using Utilities;

namespace Game
{
    public class DestroyAfterSeconds : MonoBehaviour
    {
        [SerializeField] float delay = 2;
        Timer timer;

        void Awake()
        {
            timer = new Timer(delay);
        }

        private void Update()
        {
            if (timer.IsDone)
                Destroy(gameObject);
        }
    }
}
