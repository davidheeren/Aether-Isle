using UnityEngine;
using Utilities;
using System.Linq;
using TMPro;

namespace Game
{
    public class FramerateDebugger : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        RollingBuffer<float> dtBuffer = new RollingBuffer<float>(20);

        private void Start()
        {
            InvokeRepeating(nameof(UpdateDisplay), 0.05f, 0.1f);
        }

        void UpdateDisplay()
        {
            if (text == null) return;

            float dtAvg = dtBuffer.Sum() / dtBuffer.Count;
            int fps = (int)(1 / dtAvg);

            text.text = fps.ToString();
        }

        void Update()
        {
            float dt = Time.deltaTime;

            if (!float.IsNaN(dt) && dt > 0)
                dtBuffer.Add(dt);
        }
    }
}
