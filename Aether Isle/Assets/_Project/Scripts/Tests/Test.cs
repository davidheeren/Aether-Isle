using UnityEngine;
using Utilities;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [SerializeField] float decay = 10;
        [SerializeField] Transform target;

        void Update()
        {
            transform.position = Smoothing.ExpDecay(transform.position, target.position, decay, Time.deltaTime);
        }
    }
}
