using System.IO;
using UnityEditor;
using UnityEngine;

namespace StateTree
{
    public static class CreateStateTreeTemplate
    {
        const string templateFolderName = "StateTreeTemplates";

        const string stateName = "StateTemplate.txt";
        const string stateWithDataName = "StateWithDataTemplate.txt";
        const string stateWithDataSCName = "StateWithDataSCTemplate.txt";

        const string conditionName = "ConditionTemplate.txt";
        const string conditionWithDataName = "ConditionWithDataTemplate.txt";
        const string conditionWithDataSCName = "ConditionWithDataSCTemplate.txt";

        public static void CreateScriptFromTemplate(string templateName, string type)
        {
            string templatePath = GetPath(templateName);
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, $"New{type}.cs");
        }

        static string GetPath(string templateName)
        {
            string assetsFolderPath = Application.dataPath;

            string[] folders = Directory.GetDirectories(assetsFolderPath, templateFolderName, SearchOption.AllDirectories);

            // Check if the folder was found
            if (folders.Length == 0)
            {
                Debug.LogError("Folder not found.");
                return null;
            }

            string templatePath = Path.Combine(folders[0], templateName);

            if (!File.Exists(templatePath))
            {
                Debug.LogError("Path not found");
                return null;
            }

            return templatePath;

        }

        #region State
        // STATE
        [MenuItem(itemName: "Assets/Create/StateTree/State")]
        public static void CreateStateTemplate() => CreateScriptFromTemplate(stateName, "State");

        // STATE WITH DATA
        [MenuItem(itemName: "Assets/Create/StateTree/StateWithData")]
        public static void CreateStateWithDataTemplate() => CreateScriptFromTemplate(stateWithDataName, "State");

        // STATE WITH DATA SC
        [MenuItem(itemName: "Assets/Create/StateTree/StateWithDataSC")]
        public static void CreateStateWithDataSCTemplate() => CreateScriptFromTemplate(stateWithDataSCName, "State");
        #endregion

        #region Condition
        // CONDITION
        [MenuItem(itemName: "Assets/Create/StateTree/Condition")]
        public static void CreateConditionTemplate() => CreateScriptFromTemplate(conditionName, "Condition");

        // CONDITION WITH DATA
        [MenuItem(itemName: "Assets/Create/StateTree/ConditionWithData")]
        public static void CreateConditionWithDataTemplate() => CreateScriptFromTemplate(conditionWithDataName, "Condition");

        // CONDITION WITH DATA SC
        [MenuItem(itemName: "Assets/Create/StateTree/ConditionWithDataSC")]
        public static void CreateConditionWithDataSCTemplate() => CreateScriptFromTemplate(conditionWithDataSCName, "Condition");
        #endregion
    }
}
