using CustomInspector;
using UnityEngine;
using Utilities;

namespace Game
{
    public class Test : MonoBehaviour
    {
        [Button(nameof(CalculateTargetPrediction))]
        [SerializeField] Vector2 targetPos;
        [SerializeField] Vector2 targetVel;
        [SerializeField] Vector2 startPos;
        [SerializeField] float projectileSpeed = 10;

        void CalculateTargetPrediction()
        {
            Vector2 output = Maths.TargetPrediction(targetPos, targetVel, startPos, projectileSpeed);
            print(output);
        }
    }
}
