using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler , IEndDragHandler, IDragHandler
{
    private RectTransform rectTr;

    private void Start()
    {
        rectTr = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       Debug.Log("down");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("start");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end");
    }

    public void OnDrag(PointerEventData eventData)
    {
       Debug.Log("OnDrag");
    }
}
