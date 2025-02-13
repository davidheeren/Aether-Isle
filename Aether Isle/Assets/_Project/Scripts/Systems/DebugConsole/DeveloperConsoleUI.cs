using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.InputSystem.UI;

namespace Game
{
    public class DeveloperConsoleUI : MonoBehaviour
    {
        private List<string> logs = new List<string>();

        private VisualElement root;
        private ListView listView;
        private TextField textField;

        UIDocument document;
        private InputSystemUIInputModule inputModule;
        //private bool inConsole;

        private void Awake()
        {
            if (!TryGetComponent<UIDocument>(out document))
                Debug.LogError("Document not here");

            inputModule = GameObject.FindFirstObjectByType<InputSystemUIInputModule>();
            if (inputModule == null)
                Debug.LogError("No input module");

            

            EnterConsole();
        }

        private void EnterConsole()
        {
            inputModule.enabled = false;
            document.enabled = true;
            //inConsole = true;

            root = document.rootVisualElement;

            listView = root.Q<ListView>("list-view");
            textField = root.Q<TextField>("text-field");

            textField.RegisterCallback<ChangeEvent<string>>(OntTextChanged);

            StartCoroutine(nameof(FocusCoroutine));
        }

        private void ExitConsole()
        {
            inputModule.enabled = true;
            document.enabled = false;

            textField.UnregisterCallback<ChangeEvent<string>>(OntTextChanged);
        }

        private void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                OnSubmit();
            }
        }

        private void OntTextChanged(ChangeEvent<string> evt)
        {
            Debug.Log("Text Changed");
        }

        private void OnSubmit()
        {
            AddLog(textField.value);
            textField.value = "";
            StartCoroutine(nameof(FocusCoroutine));
        }

        private void AddLog(string log)
        {
            logs.Add(log);
            listView.itemsSource = logs;
            listView.Rebuild();
        }

        // Wait 2 frames to avoid errors
        private IEnumerator FocusCoroutine()
        {
            yield return null;
            yield return null;
            textField?.Focus();
        }
    }
}
