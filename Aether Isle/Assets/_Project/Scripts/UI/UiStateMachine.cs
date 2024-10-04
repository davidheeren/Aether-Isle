using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Game
{
    public class UiStateMachine : MonoBehaviour
    {
        [SerializeField] UnityEngine.EventSystems.EventSystem eventSystem;
        [SerializeField] GameObject initialState;

        GameObject currentState;

        void Awake()
        {
            SetState(initialState);

            InputManager.Instance.input.UI.Navigate.performed += OnInput;
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance())
                InputManager.Instance.input.UI.Navigate.performed -= OnInput;
        }

        public void SetState(GameObject state)
        {
            if (currentState != null)
                currentState?.SetActive(false);

            currentState = state;

            if (currentState != null)
                currentState.SetActive(true);
            
            SetSelected();
        }

        void SetSelected()
        {
            eventSystem.SetSelectedGameObject(GetFirstChild());
        }

        private void OnInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (eventSystem.currentSelectedGameObject == null)
                SetSelected();
        }

        GameObject GetFirstChild()
        {
            return currentState?.transform.GetChild(0)?.gameObject;
        }

        public void PrintButton(Button button)
        {
            print(button.name);
        }
    }
}
