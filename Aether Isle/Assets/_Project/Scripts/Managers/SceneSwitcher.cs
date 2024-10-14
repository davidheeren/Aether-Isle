using UnityEngine;
using Utilities;
using UnityEngine.SceneManagement;
using Save;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using CustomInspector;
using System.Collections;

namespace Game
{
    public class SceneSwitcher : Singleton<SceneSwitcher>
    {
        // TODO: Switch to static class if no need for monobehaviour

        [Button(nameof(StartTransition))]
        [SerializeField] float transitionTime = 0.5f;

        Vignette vignette;
        ColorAdjustments colorAdjustments;

        private void Awake()
        {
            if (TryGetComponent<Volume>(out Volume volume))
            {
                if (volume.profile.TryGet<Vignette>(out Vignette vignette))
                    this.vignette = vignette;
                if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjustments))
                    this.colorAdjustments = colorAdjustments;
            }
        }

        void StartTransition()
        {
            StartCoroutine(nameof(Transition));
        }

        public void Restart()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("Current Scene Index: " + SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            print("Quit Application"); // Application.Quit() does nothing in the editor
            Application.Quit();
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
            //InputManager.Instance.input.Disable();
        }

        public void LoadSavedScene()
        {
            SceneManager.LoadScene(SaveSystem.Data.sceneIndex);
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
            //InputManager.Instance.input.Disable();
        }

        IEnumerator Transition()
        {
            float t = 0;

            while (t <= transitionTime)
            {                    
                if (vignette != null)
                    vignette.intensity.value = EasingFunction.EaseOutQuad(0, 1, t / transitionTime);

                if (colorAdjustments != null)
                    colorAdjustments.postExposure.value = EasingFunction.EaseOutQuad(0, -10, t / transitionTime);

                print(EasingFunction.EaseOutQuad(0, -10, t / transitionTime));

                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
