using System;
using Helpers;
using TMPro;
using UnityEngine;

namespace MenuScripts.StartingMenu
{
    public class GameConfigManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI seedText;

        private void Start()
        {
            if (SaveHelper.CurrentSaveFile is null)
            {
                Debug.LogError("No game save file selected");
                return;
            }

            seedText.text = "Current Seed: " + SaveHelper.CurrentSaveFile.SeedNumber;
        }

        public void ChangeSeedNumber()
        {
            // TODO: update current save file with new seed.
        }
    }
}
