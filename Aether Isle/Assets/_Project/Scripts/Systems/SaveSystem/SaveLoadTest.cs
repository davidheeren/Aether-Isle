using CustomInspector;
using Save;
using UnityEngine;

namespace Test
{
    public class SaveLoadTest : MonoBehaviour
    {
        [Button(nameof(Save))]
        [Button(nameof(Load))]
        [Button(nameof(CreateNew))]

        [SerializeField] SaveData saveCopy;
        [SerializeField, ReadOnly] SaveData loadCopy;

        void Save()
        {
            SaveSystem.Data = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(saveCopy));
            SaveSystem.Save();
        }

        void Load()
        {
            SaveSystem.Load();
            loadCopy = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(SaveSystem.Data));
        }

        void CreateNew()
        {
            SaveSystem.CreateNewSave();
        }
    }
}
