using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    [System.Serializable]
    public class SaveData
    {
        // Using Newtonsoft's serialization library. It only serializes public fields and properties by default
        // Use [JsonIgnore] to ignore public member and [JsonProperty] to include private member

        public DateTime realTimeAtSaveCreated = new DateTime();
        public DateTime realTimeAtLastSaved = new DateTime();
        public string gameVersion;

        public int coins;

        // Unity's Vector2 has a normalized property that Newtonsoft has trouble serializing. It also cannot do float2
        [JsonIgnore] public Vector2 PlayerSpawnPos
        {
            get
            {
                return new Vector2(playerSpawnPosX, playerSpawnPosY);
            }

            set
            {
                playerSpawnPosX = value.x;
                playerSpawnPosY = value.y;
            }
        }
        [JsonProperty] float playerSpawnPosX;
        [JsonProperty] float playerSpawnPosY;

        // Saves the Game Time when the program was exited
        // You can get the current Game Time by adding Time.time to this value
        public float timeAtLastUnload;

        public Dictionary<string, float> enemySpawnTimes = new Dictionary<string, float>();
    }
}
