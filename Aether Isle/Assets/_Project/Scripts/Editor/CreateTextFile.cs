using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class CreateTextFile : EditorWindow
    {
        [MenuItem("GameTools/Create Text File")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CreateTextFile));
        }

        private string fileName = "NewTextFile"; // Default filename

        private void OnGUI()
        {
            GUILayout.Label("Create Text File", EditorStyles.boldLabel);

            // Input field for the filename
            fileName = EditorGUILayout.TextField("File Name:", fileName);

            if (GUILayout.Button("Create New Text File") || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return))
            {
                if (string.IsNullOrEmpty(fileName))
                    return;

                string folderPath = CurrentFolderPath();

                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    Debug.LogError("Please select a valid folder in the Project window.");
                    return;
                }

                CreateNewTextFile(folderPath);
                Close();
            }
        }

        private void CreateNewTextFile(string folderPath)
        {
            // Combine the directory path and the filename
            string filePath = Path.Combine(folderPath, fileName) + ".txt";

            // Check if the file already exists
            if (File.Exists(filePath))
            {
                // If the file already exists, prompt the user to confirm overwrite
                if (!EditorUtility.DisplayDialog("File Exists", "A file with the same name already exists. Do you want to overwrite it?", "Yes", "No"))
                    return;
            }

            string defaultText = "";

            // Create the text file
            File.WriteAllText(filePath, defaultText);
            AssetDatabase.Refresh();
        }

        string CurrentFolderPath()
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }
}
