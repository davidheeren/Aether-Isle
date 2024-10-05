using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StateMachineUI : MonoBehaviour
    {
        [SerializeField] UnityEngine.EventSystems.EventSystem eventSystem;
        [SerializeField] GameObject initialState;

        public bool canUseBackInput = true;

        Stack<State> states = new Stack<State>();

        void Awake()
        {
            if (eventSystem == null)
                eventSystem = GameObject.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>();
            if (eventSystem == null)
                Debug.LogError("UI Statemachine needs an event system");

            if (initialState != null)
                SetState(initialState);

            InputManager.Instance.input.UI.Navigate.performed += OnNavigateInput;
            InputManager.Instance.input.UI.Back.performed += OnBackInput;
            InputManager.Instance.input.UI.Point.performed += OnPoint;
        }

        private void OnDestroy()
        {
            if (InputManager.HasInstance())
            {
                InputManager.Instance.input.UI.Navigate.performed -= OnNavigateInput;
                InputManager.Instance.input.UI.Back.performed -= OnBackInput;
                InputManager.Instance.input.UI.Point.performed -= OnPoint;
            }
        }

        public void Clear()
        {
            if (states.Count > 0)
                states.Peek().gameObject.SetActive(false);

            states.Clear();
            eventSystem.SetSelectedGameObject(null);
        }

        public void Back()
        {
            if (states.Count >= 2)
            {
                ExitState(states.Pop());

                states.Peek().gameObject.SetActive(true);

                SetSelected();
            }
        }

        public void SetState(GameObject state)
        {
            if (state == null)
            {
                Debug.LogError("Cannot set state to null.");
                return;
            }

            if (states.Count > 0)
                ExitState(states.Peek());

            states.Push(new State(state));

            states.Peek().gameObject.SetActive(true);

            SetSelected();
        }

        void ExitState(State state)
        {
            if (eventSystem.currentSelectedGameObject != null)
                state.lastSelected = eventSystem.currentSelectedGameObject;

            state.gameObject.SetActive(false);
        }

        void SetSelected()
        {
            if (states.Count > 0)
            {
                eventSystem.SetSelectedGameObject(states.Peek().lastSelected);
            }
        }

        private void OnNavigateInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (eventSystem.currentSelectedGameObject == null)
                SetSelected();
        }

        private void OnBackInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (canUseBackInput)
                Back();
        }
        private void OnPoint(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            print("POINT");
            if (InputManager.Instance.currentControlScheme != InputManager.ControlScheme.None)
                eventSystem.SetSelectedGameObject(null);
        }

        public void PrintButton(Button button)
        {
            print(button.name);
        }

        public class State
        {
            public GameObject gameObject;
            public GameObject lastSelected;

            public State(GameObject gameObject)
            {
                this.gameObject = gameObject;
                lastSelected = GetFirstChildSelectable(gameObject);
            }

            GameObject GetFirstChildSelectable(GameObject go)
            {
                if (go == null)
                    return null;

                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;

                    if (child.TryGetComponent<Selectable>(out Selectable v))
                    {
                        return child;
                    }
                }

                return null;
            }
        }
    }
}
