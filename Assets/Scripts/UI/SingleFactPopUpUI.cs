using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class SingleFactPopUpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI factText;
    private RectTransform popUpRectTransform;
    public event EventHandler OnPopUpCloseRequested;

    private float popUpAnimateDuration = 1f;
    private Tweener currentTween;

    private float bottomYPos;

    private void Awake()
    {
        bottomYPos = -Screen.height;
        popUpRectTransform = GetComponent<RectTransform>();
        HideInstant();
    }

    public void InitializePopUp(string _factText)
    {
        factText.text = _factText;
    }

    public void Hide() // Hiding the Pop Up with Tween animation
    {
        // Animate to move down
        currentTween = popUpRectTransform.DOAnchorPosY(bottomYPos, popUpAnimateDuration)
            .SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                ResetValues();
                gameObject.SetActive(false);
            });
    }

    private void HideInstant() //Hide the UI on Start without animations
    {
        ResetValues();
        gameObject.SetActive(false);
    }

    public void ClosePopUp()
    {
        OnPopUpCloseRequested?.Invoke(this, EventArgs.Empty);
    }

    public void Show(string _factType) // Showing the Pop Up with Tween animation
    {
        gameObject.SetActive(true);
        InitializePopUp(_factType);

        // If there's an active show animation, kill it
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // Set initial position
        popUpRectTransform.anchoredPosition = new Vector2(popUpRectTransform.anchoredPosition.x, bottomYPos);
        currentTween = popUpRectTransform.DOAnchorPosY(0, popUpAnimateDuration).SetEase(Ease.OutExpo);
    }

    private void ResetValues()
    {
        factText.text = "";
    }
}
