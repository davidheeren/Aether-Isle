using CustomInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ExecuteInEditMode]
    public class UniqueID : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField, ReadOnly] string _id;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                    Debug.LogError("Unique Id is empty");

                return _id;
            }
        }

        string idDump;


        private void Awake()
        {
            if (string.IsNullOrEmpty(_id))
                SetID();
        }

        void SetID()
        {
            // There should be one collision in every 16 quintillion hash values

            Hash128 hash = new Hash128();
            System.Random rand = new();

            hash.Append(gameObject.name);
            hash.Append(gameObject.GetInstanceID());
            hash.Append(SceneManager.GetActiveScene().name);
            hash.Append(rand.Next());
            hash.Append(rand.Next());

            _id = hash.ToString();
        }

        #region PreventResetSerialization
        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(idDump))
                _id = idDump;
        }

        public void OnBeforeSerialize()
        {
            idDump = _id;
        }
        #endregion
    }
}
