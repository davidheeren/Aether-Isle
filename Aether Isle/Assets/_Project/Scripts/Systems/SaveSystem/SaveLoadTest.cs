using CustomInspector;
using Save;
using UnityEngine;

namespace Test
{
    public class SaveLoadTest : MonoBehaviour
    {
        [Button(nameof(Save))]
        [SerializeField] SaveData saveCopy;

        [Button(nameof(Load))]
        [SerializeField, ReadOnly] SaveData loadCopy;

        [Button(nameof(Clear))]
        [Button(nameof(TestDictionary))]
        [SerializeField, ReadOnly] string button;

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

        void TestDictionary()
        {
            SaveSystem.SaveData.enemySpawnTimes.Add(Random.value.ToString(), Random.value);
            SaveSystem.Save();
        }
    }
}
