
namespace Save
{
    [System.Serializable]
    public class SaveObject
    {
<<<<<<< Updated upstream
=======
        // Using Newtonsoft's serialization library. It only serializes public fields and properties by default
        // Use [JsonIgnore] to ignore public member and [JsonProperty] to include private member

        public string version;

>>>>>>> Stashed changes
        public int coins;
    }
}
