
using Models.Combatants;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// Represents a save file for the game.
    /// must include the following properties:
    ///     - FileName: string
    ///     - Character : Character
    ///     - PlayTime : float
    /// </summary>
    [System.Serializable]
    public class SaveFile
    {
        public string FileName { get; set; }
        public string Character { get; set; }
        public float PlayTime { get; set; }

        // In the future, these can be added:
        // public List<Quest> Quests { get; set; }
        // public string location { get; set; }
        // public vector3 position { get; set; }

        public SaveFile(string fileName, Character character, float playTime)
        {
            FileName = fileName;
            Character = JsonUtility.ToJson(character);
            PlayTime = playTime;
        }

        public Character GetCharacter()
        {
            // Assuming Character is a JSON string representation of a Character object
            return JsonUtility.FromJson<Character>(Character);
        }
    }
}