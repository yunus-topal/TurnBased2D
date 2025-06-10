using Models;
using UnityEngine;

public class SaveFileUIHelper : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI saveNameText;
    private SaveFile SaveFile { get; set; }
    public void Initialize(SaveFile saveFile)
    {
        SaveFile = saveFile;
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnSaveFileSelected());
        saveNameText.text = saveFile.SaveName;
        // TODO add more fields to prefab and populate here accordingly.

    }

    private void OnSaveFileSelected()
    {
        // Save the selected save file name to PlayerPrefs
        PlayerPrefs.SetString(Helpers.Constants.lastUsedSaveKey, SaveFile.SaveName);
        PlayerPrefs.Save();

        // Optionally, you can load the game or perform other actions here
        Debug.Log($"Selected save file: {SaveFile.SaveName}");

        // Load the save file and change scene
    }
}
