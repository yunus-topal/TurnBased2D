using UnityEngine;

namespace Helpers
{
    
    // use this class for static string values.
    // can be put into a database or a json/xml file if it gets too large
    public static class Constants
    {
        // player pref keys
        public static readonly string musicKey = "music";
        public static readonly string soundKey = "sound";
        
        // ui texts
        public static readonly string musicText = "Music";
        public static readonly string soundText = "Sound";

        public static readonly string saveFileLocation = Application.persistentDataPath + "/Saves/";
        // check player prefs using this
        public static readonly string lastUsedSaveKey = "lastUsedSave";

    }
}