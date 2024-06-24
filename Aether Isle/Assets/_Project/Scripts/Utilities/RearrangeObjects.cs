using CustomInspector;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.Tools
{
    public class RearrangeObjects : MonoBehaviour
    {
        [Button(nameof(CheckCount))]
        [Button(nameof(Rearrange))]

        [TooltipBox("Only finds name with containsName that can also have *space*(anyNumber) at the end")]
        [SerializeField] string containsName;


        [Space(20)]
        [SerializeField] bool canRename;
        [SerializeField] string newName;
    
    
        void CheckCount()
        {
            if (containsName == "")
            {
                Debug.LogWarning("no name");
                return;
            }
    
            List<GameObject> filteredObjects = FilterObjects();
    
            print("Filtered Objects count: " + filteredObjects.Count);
        }
    
        void Rearrange()
        {
            if (containsName == null)
            {
                Debug.LogWarning("No name");
                return;
            }
    
            List<GameObject> filteredObjects = FilterObjects();
    
            for (int i = 0;i < filteredObjects.Count; i++)
            {
                if (canRename && newName != "")
                    containsName = newName;

                filteredObjects[i].gameObject.name = containsName + " (" + i + ")";
            }
        }
    
        List<GameObject> FilterObjects()
        {
            GameObject[] objects = FindObjectsByType<GameObject>(0);
    
            objects = HierarchicalSorting.Sort(objects);
    
            List<GameObject> filteredObjects = new List<GameObject>();
    
            for (int i = 0; i < objects.Length; i++)
            {
                string objectName = objects[i].name;
    
                objectName = FilterString(objectName);
    
                if (objectName == containsName)
                    filteredObjects.Add(objects[i]);
            }
    
            return filteredObjects;
        }
    
        string FilterString(string filterString)
        {
            string pattern = @" \(\d+\)$"; // space: parenthesis: any numbers: parenthesis: anchored at the end
    
            filterString = Regex.Replace(filterString, pattern, "");
    
            return filterString;
        }
    }
}
