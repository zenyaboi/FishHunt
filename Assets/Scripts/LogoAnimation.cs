using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public float speed = 5f;
    public float height = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = rectTransform.localPosition;
        float newY = Mathf.Sin(Time.time * speed) * height;
        rectTransform.localPosition = new Vector3(pos.x, newY + 215, pos.z);
    }
}
