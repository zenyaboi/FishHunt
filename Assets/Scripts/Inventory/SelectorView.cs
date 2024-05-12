using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorView : MonoBehaviour
{
    private RectTransform _rectTransform;
    [SerializeField] private float _speed = 10f;

    private void Start()
    {
        // Getting the RectTransform component from Selector
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update() 
    {
        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null) return;

        transform.position = Vector3.Lerp(transform.position, selected.transform.position, _speed * Time.deltaTime);

        // Getting the selected gameobject RectTransform
        var otherRect = selected.GetComponent<RectTransform>();

        // Scaling the selector according the size of the slot
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, otherRect.rect.size.x);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, otherRect.rect.size.y);
    }
}
