using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;
    private Vector2 inputVector;
    public Action<float, float> OnJoystickMove;

    void Start()
    {
        handle = GetComponent<RectTransform>();    
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle, eventData.position, eventData.pressEventCamera, out pos);
        pos.x = pos.x / background.sizeDelta.x;
        pos.y = pos.y / background.sizeDelta.y;

        inputVector = new Vector2(Mathf.Clamp(pos.x, -1f, 1f), Mathf.Clamp(pos.y, -1f, 1f));

        handle.anchoredPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 2), inputVector.y * (background.sizeDelta.y / 2));

        OnJoystickMove?.Invoke(inputVector.x, inputVector.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        OnJoystickMove?.Invoke(0, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }
}
