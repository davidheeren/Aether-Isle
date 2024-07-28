using CustomInspector;
using UnityEngine;
using Utilities;

namespace Game
{
    public class Test : MonoBehaviour
    {
<<<<<<< Updated upstream
        [Button(nameof(Slow))]
        [Button(nameof(Fast))]
        [SerializeField] Vector2 v;

=======
        [SerializeField] LayerMask mask;
>>>>>>> Stashed changes

        void Slow()
        {
<<<<<<< Updated upstream
            print(v.normalized / v.magnitude);
        }

        void Fast()
        {
            print(v.Reciprocal());
=======
            mask = gameObject.layer.GetLayerMask();
>>>>>>> Stashed changes
        }
    }
}
