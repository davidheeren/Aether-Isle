using Save;
using UnityEngine;
using Utilities;

namespace Game
{
    public class TimeManager : Singleton<TimeManager>
    {
        private void OnApplicationQuit()
        {
            SaveSystem.Data.timeAtLastUnload += Time.time;
            //SaveSystem.SaveObject.PlayerPos = new Vector2(10, -8);
            SaveSystem.Save();
            //print("Exit Application");
        }
    }
}
