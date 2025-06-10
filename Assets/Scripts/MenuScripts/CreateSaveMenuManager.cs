using UnityEngine;
using UnityEngine.UI;
using Helpers;
using Models;

public class CreateSaveMenuManager : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button createButton;
    [SerializeField] private GameObject saveFilePrefab;
    [SerializeField] private Transform saveFileListContainer;

    [SerializeField] private GameObject CreateSaveInput;
    private void OnEnable()
    {
        CreateSaveInput.SetActive(false);
        // check if there is a valid save file in the player prefs.
        var lastUsedSave = PlayerPrefs.GetString(Helpers.Constants.lastUsedSaveKey, string.Empty);
        // try to load the last used save file
        var lastSave = Helpers.SaveHelper.LoadSaveFileFromFileName(lastUsedSave);
        if (lastSave == null)
        {
            backButton.gameObject.SetActive(false);
        }

        // get all save files.
        var saveFiles = Helpers.SaveHelper.GetAllSaveFiles();
        // populate list with save files
        foreach (var saveFile in saveFiles)
        {
            var saveFileObject = Instantiate(saveFilePrefab, transform);
            saveFileObject.transform.SetParent(saveFileListContainer, false); // Set parent without changing local scale/position

        }
    }

    public void OnCreateNewSaveClicked()
    {
        CreateSaveInput.SetActive(true);
    }


}
