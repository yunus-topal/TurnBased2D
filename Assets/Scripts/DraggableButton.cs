using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private BaseMenuManager _baseMenuManager;
    private Vector3 _originalPosition;
    private void Start() {
        _baseMenuManager = FindObjectOfType<BaseMenuManager>();
        _originalPosition = transform.position;
    }
    public void ResetPosition() {
        transform.position = _originalPosition;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }
    
    // ask manager to snap button to the nearest position
    // otherwise, return button to its original position
    public void OnEndDrag(PointerEventData eventData) {
        if(_baseMenuManager.TryToSnap(this)) return;
        transform.position = _originalPosition;
    }
}
