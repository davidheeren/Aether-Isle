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

        // For final version:
        // use Application.persistentDataPath for save path
        // Use base64 encoding to deter cheating

        // Location should be in %appdata% in windows
        static readonly string saveFolderPath = Application.persistentDataPath + "/Saves";
        static readonly string saveFilePath = saveFolderPath + "/Save.json";

        static readonly bool canDebug = false;

        public static event Action OnSave;

        static SaveData _saveData;
        public static SaveData SaveData
        {
            get
            {
                if (_saveData == null)
                    Load();

#if UNITY_EDITOR
                // The instance is static so if we delete the file in the editor without recompiling weird things can happen
                if (!File.Exists(saveFilePath))
                {
                    Load();
                    Debug.LogWarning("Create new save because the save file was deleted without recompiling");
                }  
#endif

                return _saveData;
            }

            set => _saveData = value;
        }

        public static void Save()
        {
            CheckSaveFile();

            if (_saveData == null)
            {
                Debug.LogError("Cannot save a null object");
                return;
            }

            _saveData.realTimeAtLastSaved = System.DateTime.Now;
            
            string jsonData = JsonConvert.SerializeObject(_saveData); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);

            OnSave?.Invoke();
            Log("Saved Data");
        }


        public static void Load()
        {
            CheckSaveFile();

            string jsonData = File.ReadAllText(saveFilePath);

            _saveData = JsonConvert.DeserializeObject<SaveData>(jsonData); // Changed to Json.Net

            Log("Loaded Data");
        }

        static void CheckSaveFile() // ensures that there will always be a save file
        {
            if (!Directory.Exists(saveFolderPath))
                Directory.CreateDirectory(saveFolderPath);

            if (!File.Exists(saveFilePath))
            {
                CreateNewSave();

                Log("Created new Save File");
            }
        }

        public static void Clear()
        {
            _saveData = new SaveData();

            string jsonData = JsonConvert.SerializeObject(_saveData); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);
        }

        static void CreateNewSave()
        {
            _saveData = new SaveData();

            _saveData.realTimeAtSaveCreated = System.DateTime.Now;
            _saveData.realTimeAtLastSaved = System.DateTime.Now;
            _saveData.gameVersion = Application.version;

            string jsonData = JsonConvert.SerializeObject(_saveData); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);
        }

        static void Log(string msg)
        {
            if (canDebug)
                Debug.Log(msg);
        }
    }
}
