using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RefreshFactsUI : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler OnRefreshButtonClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnRefreshButtonClick?.Invoke(this, EventArgs.Empty);
    }
}
