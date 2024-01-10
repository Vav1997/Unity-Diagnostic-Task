using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class SingleFactUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI factTypeText;
    public event Action<SingleFactUI> OnSignleFactUIClicked;

    public void InitializeValues(string _factType)
    {
        factTypeText.text = $"Type: {_factType}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSignleFactUIClicked?.Invoke(this);
    }


}
