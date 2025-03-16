using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]public Item item;
    public int count = 1;
    public Text countText;
    public Image image;
    public Transform parentAfterDrag;

    
    
    public void InitItem(Item newitem)
    {
        item = newitem;
        image.sprite = newitem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    } 

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }


    public void OnEndDrag(PointerEventData eventData)
    {
        
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
    
}
