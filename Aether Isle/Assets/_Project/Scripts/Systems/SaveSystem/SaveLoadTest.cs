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

        void Save()
        {
            SaveSystem.SaveObject = saveCopy.CopyJson<SaveObject>();
        }

        void Load()
        {
            loadCopy = SaveSystem.SaveObject.CopyJson<SaveObject>();
        }
    }
}
