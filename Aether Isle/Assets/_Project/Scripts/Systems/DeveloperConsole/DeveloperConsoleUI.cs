using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Collections;
using Game;
using System.Linq;


namespace DeveloperConsole
{
    public class DeveloperConsoleUI : MonoBehaviour
    {
        private List<string> logs = new List<string>();

        private DeveloperConsole console;

        private UIDocument document;
        private VisualElement root;
        private ListView listView;
        private TextField textField;
        private ListView hints;

        private bool inConsole;

        //private int historyIndex = -1;
        //private int completionIndex = -1;

        // TODO: history and auto-completion

        private void Start()
        {
            if (!TryGetComponent<UIDocument>(out document))
                Debug.LogError("Document not here");

            if (!TryGetComponent<DeveloperConsole>(out console))
                Debug.LogError("DeveloperConsoleUI needs a DeveloperConsole component");
        }

        private void EnterConsole()
        {
            InputManager.Instance.input.Game.Disable();
            Time.timeScale = 0;

            document.enabled = true;
            inConsole = true;

            root = document.rootVisualElement;

            listView = root.Q<ListView>("list-view");
            textField = root.Q<TextField>("text-field");
            hints = root.Q<ListView>("hints");

            textField.RegisterCallback<ChangeEvent<string>>(OntTextChanged);
            // text field only has an key down event not submit

            SetHintsToHelp();
            ClearConsole();
            textField.value = "";

            StartCoroutine(nameof(FocusCoroutine));
        }

        private void ExitConsole()
        {
            InputManager.Instance.input.Game.Enable();
            Time.timeScale = 1;

            document.enabled = false;
            inConsole = false;

            textField.UnregisterCallback<ChangeEvent<string>>(OntTextChanged);

            console.history.Clear();
        }

        private void Update()
        {
            if (InputManager.Instance.input.Developer.Console.WasPerformedThisFrame())
            {
                if (!inConsole)
                    EnterConsole();
                else
                    ExitConsole();
            }

            if (Keyboard.current.enterKey.wasPressedThisFrame && inConsole)
            {
                OnSubmit();
            }
        }

        private void OntTextChanged(ChangeEvent<string> evt)
        {
            // Empty Hints
            if (string.IsNullOrEmpty(textField.text))
            {
                SetHintsToHelp();
                return;
            }

            // Info command hints
            string[] inputs = console.SplitAndFormat(textField.text);
            string infoID = "info";
            if (console.commands.ContainsKey(infoID) && inputs[0] == infoID)
            {
                if (inputs.Length == 2)
                {
                    SetHints(console.GetCommandListFromPrefix(inputs[1]));
                }
                else if (inputs.Length == 1)
                {
                    SetHints(console.GetCommandListFromPrefix(""));
                }
                else
                {
                    SetHints(null);
                }
                return;
            }

            // Default hints
            SetHints(console.GetCommandListFromPrefix(textField.text));
        }

        private void OnSubmit()
        {
            if (string.IsNullOrEmpty(textField.text))
                return;

            PrintConsole("# " +  textField.value);

            console.UpdateConsole(textField.text);

            textField.value = "";
            StartCoroutine(nameof(FocusCoroutine));
        }


        //Wait 2 frames to avoid errors
        private IEnumerator FocusCoroutine()
        {
            yield return null;
            yield return null;
            textField?.Focus();
        }


        public void PrintConsole(string text)
        {
            if (listView.itemsSource == null)
            {
                listView.itemsSource = new List<string>() { text };
            }
            else
            {
                listView.itemsSource.Add(text);
            }

            listView.RefreshItems();

            listView.schedule.Execute(() =>
            {
                if (listView.itemsSource != null && listView.itemsSource.Count > 0)
                {
                    listView.ScrollToItem(listView.itemsSource.Count - 1);
                }
            }).ExecuteLater(50);
        }

        public void ClearConsole()
        {
            listView.itemsSource = null;
            listView.RefreshItems();
        }

        private void SetHints(List<string> h)
        {
            if (h != null && h.Count == 0)
                hints.itemsSource = null;
            else
                hints.itemsSource = h;

            hints.RefreshItems();
        }

        private void SetHintsToHelp()
        {
            if (!console.commands.TryGetValue("help", out IDeveloperCommand c))
            {
                SetHints(null);
                return;
            }

            SetHints(new List<string>() { console.CommandInfo(c) });
        }

        private void SetHintsToInfo(string[] inputs)
        {
            SetHints(console.commands.Values.Select(x => x.ID).ToList());
        }
    }
}
