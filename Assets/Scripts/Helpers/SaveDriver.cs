using Models;
using UnityEngine;

namespace Helpers
{
    public class SaveDriver : MonoBehaviour
    {
        //public SaveFile StartingData;   // optional default

        //private SaveFile _currentSave;

        //private void Awake()
        //{
        //    // Try load, or fall back to StartingData
        //    _currentSave = SaveManager.Load<SaveFile>()
        //                   ?? StartingData;

        //    // TODO: populate your world from _currentSave...
        //}

        //private void OnApplicationQuit()
        //{
        //    // Gather data from your game world into a SaveFile
        //    var save = GatherCurrentSaveFile();
        //    SaveManager.Save(save);
        //}

        //private SaveFile GatherCurrentSaveFile()
        //{
        //    // pull your Character[] etc. into a SaveFile instance
        //    // …
        //}
    }
}