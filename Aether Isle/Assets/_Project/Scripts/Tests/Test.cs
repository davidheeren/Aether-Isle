using CustomInspector;
using UnityEngine;
using Utilities;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [Button(nameof(Slow))]
        [Button(nameof(Fast))]
        [SerializeField] Vector2 v;


        void Slow()
        {
            print(v.normalized / v.magnitude);
        }

        void Fast()
        {
            print(v.Reciprocal());
        }
    }
}
