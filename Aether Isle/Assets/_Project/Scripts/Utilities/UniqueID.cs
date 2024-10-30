using CustomInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ExecuteInEditMode]
    public class UniqueID : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField, ReadOnly] string _id;
        [SerializeField, ReadOnly] bool newIdOnDuplicate = true;

        [SerializeField, Hook(nameof(SetOverrideID)), ShowIf(nameof(canOverride))] string overrideID;
        bool canOverride = false;

        string idDump;

        static List<GameObject> lastCreatedGameObjects = new List<GameObject>();

        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                    Debug.LogError("Unique Id is empty", gameObject);

                return _id;
            }
        }

        private void Reset()
        {
            if (string.IsNullOrEmpty(_id))
                SetID();
        }

        private void Start()
        {
            if (!newIdOnDuplicate)
                return;

            foreach (GameObject go in lastCreatedGameObjects)
            {
                if (go == gameObject)
                    SetID();
            }
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            // Event calls between Awake and Start
            ObjectChangeEvents.changesPublished += ChangesPublished;
        }

        // https://discussions.unity.com/t/callback-when-object-is-created-in-editor/509531/13
        // Whenever there are gameobjects duplicated in the editor clear then save a list of all of the gameobjects
        private static void ChangesPublished(ref ObjectChangeEventStream eventStream)
        {
            lastCreatedGameObjects.Clear();

            for (int i = 0; i < eventStream.length; ++i)
            {
                var type = eventStream.GetEventType(i);

                if (type == ObjectChangeKind.CreateGameObjectHierarchy)
                {
                    eventStream.GetCreateGameObjectHierarchyEvent(i, out var createGameObjData);
                    GameObject createdGameObject = EditorUtility.InstanceIDToObject(createGameObjData.instanceId) as GameObject;

                    //this is null while in Prefab mode (Preview scene). Leaving the null check for any future unexpected cases
                    //if (createdGameObject == null || EditorSceneManager.IsPreviewScene(createdGameObject.scene))
                    //    return;

                    //print("CALLBACK");
                    lastCreatedGameObjects.Add(createdGameObject);
                }
            }
        }
#endif


        [ContextMenu("Set New ID")]
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
            idDump = _id;

            Debug.Log("Set ID", gameObject);
        }

        [ContextMenu("Toggle New On Duplicate")]
        void ToggleNewOnDuplicate()
        {
            newIdOnDuplicate = !newIdOnDuplicate;
        }

        [ContextMenu("Toggle Can Override")]
        void ToggleCanOverride()
        {
            canOverride = !canOverride;
        }

        void SetOverrideID()
        {
            if (!string.IsNullOrEmpty(overrideID))
            {
                _id = overrideID;
                idDump = _id;
            }
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
