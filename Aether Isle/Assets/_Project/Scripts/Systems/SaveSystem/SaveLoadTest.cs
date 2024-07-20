using CustomInspector;
using Save;
using UnityEngine;

namespace Test
{
    public class SaveLoadTest : MonoBehaviour
    {
        [Button(nameof(Save))]
        [SerializeField] SaveObject saveCopy;

        [Button(nameof(Load))]
        [SerializeField, ReadOnly] SaveObject loadCopy;

        [Button(nameof(Clear))]
        [Button(nameof(TestDictionary))]
        [SerializeField, ReadOnly] string button;

        void Save()
        {
            SaveSystem.SaveObject = JsonUtility.FromJson<SaveObject>(JsonUtility.ToJson(saveCopy));
            SaveSystem.Save();
        }

        void Load()
        {
            SaveSystem.Load();
            loadCopy = JsonUtility.FromJson<SaveObject>(JsonUtility.ToJson(SaveSystem.SaveObject));
        }

        void Clear()
        {
            SaveSystem.Clear();
        }

        void TestDictionary()
        {
            SaveSystem.SaveObject.enemySpawnTimes.Add(Random.value.ToString(), Random.value);
            SaveSystem.Save();
        }
    }
}
