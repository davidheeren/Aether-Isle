using System;
using System.IO;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        //static readonly string saveFolderPath = Application.persistentDataPath + "/Saves"; // More secure but won't show in assets

        static readonly string saveFolderPath = Application.dataPath + "/Saves";
        static readonly string saveFilePath = saveFolderPath + "/Save.txt";

        public static event Action OnSave;

        static SaveObject _saveObject;
        public static SaveObject SaveObject
        {
            get
            {
                CheckSaveFile();

                if (_saveObject == null)
                {
                    _saveObject = new SaveObject();
                    LoadData();
                }

                return _saveObject;
            }
            set
            {

                _saveObject = value;
                CheckSaveFile();
                SaveData();
            }
        }

        static void SaveData()
        {
            if (_saveObject == null)
            {
                Debug.LogError("Cannot save a null object");
                return;
            }

            string jsonData = JsonUtility.ToJson(_saveObject);

            File.WriteAllText(saveFilePath, jsonData);

            OnSave?.Invoke();
            Debug.Log("Saved Data");
        }


        static void LoadData()
        {
            string jsonData = File.ReadAllText(saveFilePath);

            _saveObject = JsonUtility.FromJson<SaveObject>(jsonData);

<<<<<<< Updated upstream
            Debug.Log("Loaded Data");
=======
            if (_saveObject.version != Application.version)
                UpdateSaveVersion();

            Log("Loaded Data");
>>>>>>> Stashed changes
        }

        static void CheckSaveFile() // ensures that there will always be a save file
        {
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            if (!File.Exists(saveFilePath))
            {
                // Makes a blank file
                _saveObject = new SaveObject();
                string jsonData = JsonUtility.ToJson(_saveObject);

                // Creates new text file and writes to it
                File.WriteAllText(saveFilePath, jsonData); // also replaces a file already there but this won't happed because we check

                Debug.Log("Created new Save File");
            }
        }
<<<<<<< Updated upstream
=======

        public static void Clear()
        {
            _saveObject = new SaveObject();
            _saveObject.version = Application.version;

            string jsonData = JsonConvert.SerializeObject(_saveObject); // Changed to Json.Net

            File.WriteAllText(saveFilePath, jsonData);
        }

        static void UpdateSaveVersion()
        {
            // Update old save version logic
            _saveObject.version = Application.version;
            Save();

            Log("Updated Old save Version");
        }

        static void Log(string msg)
        {
            if (canDebug)
                Debug.Log(msg);
        }
>>>>>>> Stashed changes
    }
}
