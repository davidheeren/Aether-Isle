using UnityEngine;
using Utilities;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [SerializeField] LayerMask mask;

        private void Awake()
        {
            mask = mask.GetLayerMaskByName("Enemy");
        }

        private void Update()
        {

        }
    }
}
