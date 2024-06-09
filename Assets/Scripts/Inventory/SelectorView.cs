using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorView : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private GameObject _selected;
    [SerializeField] private RectTransform _selectedPos;

    private void Start()
    {
        // Getting the RectTransform component from Selector
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update() 
    {
        // Making sure we the selected variable is never null, so we are always with something selected
        var selectedGameObject = EventSystem.current.currentSelectedGameObject;
        _selected = (selectedGameObject == null) ? _selected : selectedGameObject;
        
        EventSystem.current.SetSelectedGameObject(_selected);

        if (_selected == null) {
            this._rectTransform.localPosition = _selectedPos.localPosition;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, _selected.transform.position, _speed * Time.deltaTime);

        // Getting the selected gameobject RectTransform
        var otherRect = _selected.GetComponent<RectTransform>();

        // Scaling the selector according the size of the slot
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, otherRect.rect.size.x);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, otherRect.rect.size.y);
    }
}
