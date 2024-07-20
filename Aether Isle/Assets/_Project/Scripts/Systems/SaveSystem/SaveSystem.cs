using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        //static readonly string saveFolderPath = Application.persistentDataPath + "/Saves"; // More secure but won't show in assets
        // JsonUtility.ToJson and JsonUtility.FromJson won't work for Dictionaries
        // Instead we now use JsonConvert.SerializeObject and JsonConvert.DeserializeObject

        static readonly string saveFolderPath = Application.dataPath + "/Saves";
        static readonly string saveFilePath = saveFolderPath + "/Save.json";

        static readonly bool canDebug = false;

        public static event Action OnSave;

        static SaveObject _saveObject;
        public static SaveObject SaveObject
        {
            get
            {
                if (_saveObject == null)
                {
                    Load();
                }

                #if UNITY_EDITOR
                if (!File.Exists(saveFilePath)) // Change the check path eventually bc it won't be needed 
                {
                    Load();
                }
                #endif

                return _saveObject;
            }

            set
            {
                _saveObject = value;
            }
        }

        public static void Save()
        {
            CheckSaveFile();

            if (_saveObject == null)
            {
                Debug.LogError("Cannot save a null object");
                return;
            }

            string jsonData = JsonConvert.SerializeObject(_saveObject); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);

            OnSave?.Invoke();
            Log("Saved Data");
        }


        public static void Load()
        {
            CheckSaveFile();

            string jsonData = File.ReadAllText(saveFilePath);

            _saveObject = JsonConvert.DeserializeObject<SaveObject>(jsonData); // Changed to Json.Net

            Log("Loaded Data");
        }

        static void CheckSaveFile() // ensures that there will always be a save file
        {
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            if (!File.Exists(saveFilePath))
            {
                Clear();

                Log("Created new Save File");
            }
        }

        public static void Clear()
        {
            _saveObject = new SaveObject();

            string jsonData = JsonConvert.SerializeObject(_saveObject); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);
        }

        static void Log(string msg)
        {
            if (canDebug)
                Debug.Log(msg);
        }
    }
}
