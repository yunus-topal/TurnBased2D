
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseMenuManager : MonoBehaviour {
    private PreparationManager _preparationManager;
    [SerializeField] private GameObject baseMenuPanel;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private GameObject draggableButtonContainer;
    [SerializeField] private GameObject draggableButtonPrefab;
    [SerializeField] private Character[] characters;
    [SerializeField] private Transform[] snapPositions;
    private DraggableButton[] _snappedButtons;

    private void Start() {
        _preparationManager = FindObjectOfType<PreparationManager>();
        _snappedButtons = new DraggableButton[snapPositions.Length];
        /*
        foreach (Character character in characters) {
            GameObject o = Instantiate(draggableButtonPrefab, Vector3.zero, Quaternion.identity, draggableButtonContainer.transform);
            o.GetComponent<Image>().sprite = character.CharSprite;
        }
        */
    }

    // lots of loops, not efficient
    public bool TryToSnap(DraggableButton button) {
        int snapIndex = -1;
        // find new snap position
        for (int i = 0; i < snapPositions.Length; i++) {
            if (Vector3.Distance(button.transform.position, snapPositions[i].position) < 100) {
                snapIndex = i;
                break;
            }
        }
        // clear button from previous snap position
        for (int i = 0; i < _snappedButtons.Length; i++) {
            if (_snappedButtons[i] == button) {
                _snappedButtons[i] = null;
                break;
            }
        }
        // snap button to new position
        if(snapIndex >= 0) {
            if(_snappedButtons[snapIndex] != null) _snappedButtons[snapIndex].ResetPosition();
            
            _snappedButtons[snapIndex] = button;
            button.transform.position = snapPositions[snapIndex].position;
            return true;
        }
        return false;
    }

    public void Proceed() {
        // check if all buttons are snapped
        foreach (var button in _snappedButtons) {
            warningText.gameObject.SetActive(true);
            if (button == null) return;
        }
        warningText.gameObject.SetActive(false);
        baseMenuPanel.SetActive(false);
        _preparationManager.SetCharacterIcons(_snappedButtons);
    }
}
