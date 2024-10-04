using CustomInspector;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        //static readonly string saveFolderPath = Application.persistentDataPath + "/Saves"; // More secure but won't show in assets
        // JsonUtility.ToJson and JsonUtility.FromJson won't work for Dictionaries
        // Instead we now use JsonConvert.SerializeObject and JsonConvert.DeserializeObject

        // For final version:
        // use Application.persistentDataPath for save path
        // Use base64 encoding to deter cheating

        // Location should be in %appdata% in windows
        static readonly string saveFolderPath = Application.persistentDataPath + "/Saves";
        static readonly string saveFilePath = saveFolderPath + "/Save.json";

        static readonly bool canDebug = false;

        public static event Action OnSave;
        public static event Action OnCreate;

        static SaveData _data;
        public static SaveData Data
        {
            get
            {
                if (_data == null)
                    Load();

#if UNITY_EDITOR
                // The instance is static so if we delete the file in the editor without recompiling weird things can happen
                if (!File.Exists(saveFilePath))
                {
                    Load();
                    Debug.LogWarning("Create new save because the save file was deleted without recompiling");
                }  
#endif

                return _data;
            }

            set => _data = value;
        }

        public static void Save()
        {
            CheckSaveFile();

            if (_data == null)
            {
                Debug.LogError("Cannot save a null object");
                return;
            }

            OnSave?.Invoke();

            _data.realTimeAtLastSaved = System.DateTime.Now;
            
            string jsonData = JsonConvert.SerializeObject(_data); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);

            Log("Saved Data");
        }

        public static void Load()
        {
            CheckSaveFile();

            string jsonData = File.ReadAllText(saveFilePath);

            _data = JsonConvert.DeserializeObject<SaveData>(jsonData); // Changed to Json.Net

            if (_data.gameVersion != Application.version) // NEW
            {
                CreateNewSave();
                Debug.Log("Save versions do not match so created new");
            }

            Log("Loaded Data");
        }

        static void CheckSaveFile() // ensures that there will always be a save file
        {
            CheckDirectory();

            if (!File.Exists(saveFilePath))
            {
                CreateNewSave();

                Log("Created new Save File");
            }
        }

        static void CheckDirectory()
        {
            if (!Directory.Exists(saveFolderPath))
                Directory.CreateDirectory(saveFolderPath);
        }

        //[MenuItem("Save/Clear")]
        //public static void Clear()
        //{
        //    CheckDirectory();
        //    _saveData = new SaveData();

        //    string jsonData = JsonConvert.SerializeObject(_saveData); // Changed to Json.Net

        //    File.WriteAllText(saveFilePath, jsonData);
        //}

        [MenuItem("Save/CreateNew")]
        public static void CreateNewSave()
        {
            CheckDirectory();

            _data = new SaveData();

            _data.realTimeAtSaveCreated = System.DateTime.Now;
            _data.realTimeAtLastSaved = System.DateTime.Now;
            _data.gameVersion = Application.version;

            string jsonData = JsonConvert.SerializeObject(_data); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);

            OnCreate?.Invoke();
        }

        [MenuItem("Save/OpenLocation")]
        public static void OpenSaveLocation() // I have not tested the Mac version
        {
            CheckDirectory();

            try
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                System.Diagnostics.Process.Start("explorer.exe", saveFolderPath.Replace("/", "\\"));
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                System.Diagnostics.Process.Start("open", saveFolderPath.Replace("/", "\\"));
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to open folder: " + ex.Message);
            }
        }

        static void Log(string msg)
        {
            if (canDebug)
                Debug.Log(msg);
        }
    }
}
