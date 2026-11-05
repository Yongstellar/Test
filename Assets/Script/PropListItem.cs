using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TMP_Text label;
    
    [SerializeField] private Image iconImage;
    
    public Proplist Data { get; private set; }

    public event Action<Proplist, PointerEventData> DragBegan;
    public event Action<Proplist, PointerEventData> Dragging;
    public event Action<Proplist, PointerEventData> DragEnded;

    public void Setup(Proplist data)
    {
        Data = data;

        if (label != null)
            label.text = data.DisplayName;

        if (iconImage != null)
        {
            iconImage.sprite = data.Icon;
            iconImage.enabled = data.Icon != null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragBegan?.Invoke(Data, eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Dragging?.Invoke(Data, eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        DragEnded?.Invoke(Data, eventData);
    }

}
