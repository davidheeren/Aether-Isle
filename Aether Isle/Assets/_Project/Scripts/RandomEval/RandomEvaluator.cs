using System;
using System.Diagnostics;

namespace RandomEval
{
    public class RandomEvaluator<T> where T : RandomEvaluation
    {
        readonly T[] evaluations;
        readonly float totalWeight = 0;

        public RandomEvaluator(params T[] evaluations)
        {
            if (evaluations == null) throw new ArgumentNullException("Evaluations array is null");
            if (evaluations.Length == 0) throw new ArgumentException("Evaluations array has 0 entries");

            this.evaluations = evaluations;

            foreach (T eval in evaluations)
            {
                if (eval == null) throw new ArgumentNullException("One of the evaluations is null");
                if (eval.EvaluationWeight < 0) throw new ArgumentOutOfRangeException("EvaluationWeight is <= 0");
                totalWeight += eval.EvaluationWeight;
            }
        }

        public T Evaluate()
        {
            float value = UnityEngine.Random.value * totalWeight;
            float cumulative = 0;

            foreach (T eval in evaluations)
            {
                cumulative += eval.EvaluationWeight;
                if (value <= cumulative)
                {
                    return eval;
                }
            }

            UnityEngine.Debug.LogError("Something went wrong with the evaluation");
            return evaluations[0];
        }
    }

    public interface RandomEvaluation
    {
        public float EvaluationWeight { get; }
    }
}
