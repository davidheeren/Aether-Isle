using CustomInspector;
using Save;
using UnityEngine;

namespace Test
{
    public class SaveLoadTest : MonoBehaviour
    {
        [Button(nameof(Save))]
        [Button(nameof(Load))]
        [Button(nameof(Clear))]

        [SerializeField] SaveData saveCopy;
        [SerializeField, ReadOnly] SaveData loadCopy;

        void Save()
        {
            SaveSystem.SaveData = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(saveCopy));
            SaveSystem.Save();
        }

        void Load()
        {
            SaveSystem.Load();
            loadCopy = JsonUtility.FromJson<SaveData>(JsonUtility.ToJson(SaveSystem.SaveData));
        }

        void Clear()
        {
            SaveSystem.Clear();
        }
    }
}
