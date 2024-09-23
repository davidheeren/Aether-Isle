using UnityEditor;
using UnityEngine;

namespace Inventory
{
    [CustomEditor(typeof(ItemDatabase))]
    [CanEditMultipleObjects]
    public class ItemDatabaseEditor : Editor
    {
        SerializedProperty itemsProperty;
        ItemDatabase itemDatabase;

        const string propertyName = "items";

        void OnEnable()
        {
            itemsProperty = serializedObject.FindProperty(propertyName);
            itemDatabase = target as ItemDatabase;

            if (itemsProperty == null) Debug.LogError("property name:" + propertyName + " not found");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(itemsProperty);

            bool itemsChanged = false;
            if (EditorGUI.EndChangeCheck())
            {
                itemDatabase.OnItemsChanged();
                itemsChanged = true;
            }

            serializedObject.ApplyModifiedProperties();

            if (itemsChanged)
                itemDatabase.OnItemsChangesApplied();
        }

        private void OnDisable()
        {
            itemDatabase.OnEditorDisable();
        }
    }
}
