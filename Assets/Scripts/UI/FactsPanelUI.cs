using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FactsPanelUI : MonoBehaviour
{
    [SerializeField] private FactsController factsController;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private SingleFactUI singleFactUIPrefab;
    [SerializeField] private RefreshFactsUI refreshFactsUI;
    private CanvasGroup factsPanelCanvasGroup;

    private List<SingleFactUI> SingleFactUIList = new List<SingleFactUI>();
    public event Action<int> OnFactPopUpRequested;
    public event EventHandler OnRefreshFactsRequested;

    private float scaleDuration = 0.5f;
    private float delayBetweenSpawns = 0.05f;
    private float inactivePanelOpacity = 0.35f;

    private void OnEnable()
    {
        refreshFactsUI.OnRefreshButtonClick += refreshFactsUI_OnRefreshButtonClick;
    }

    private void Awake()
    {
        factsPanelCanvasGroup = GetComponent<CanvasGroup>();
    }
    private void refreshFactsUI_OnRefreshButtonClick(object sender, EventArgs e)
    {
        OnRefreshFactsRequested?.Invoke(this, EventArgs.Empty);
    }

    public void StartEraseFactsWithDelay(Action onCompleteCallback = null)
    {
        StartCoroutine(EraseFactsWithDelay(onCompleteCallback));
    }
    public IEnumerator EraseFactsWithDelay(Action onCompleteCallback = null)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (SingleFactUI singleFactUI in SingleFactUIList)
        {
            Tween scaleTween = singleFactUI.transform.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.InOutSine);
            sequence.Join(scaleTween); 
        }

        yield return sequence.WaitForCompletion();

        foreach (SingleFactUI singleFactUI in SingleFactUIList)
        {
            singleFactUI.OnSignleFactUIClicked -= singleFactUI_OnSignleFactUIClicked;
            Destroy(singleFactUI.transform.gameObject);
        }

        SingleFactUIList.Clear();
        onCompleteCallback?.Invoke();
    }

    public void DrawFacts(List<Fact> factsList, Action onCompleteCallback = null)
    {
        StartCoroutine(SpawnFactsWithDelay(factsList, onCompleteCallback));
    }

    private IEnumerator SpawnFactsWithDelay(List<Fact> factsList, Action onCompleteCallback = null)
    {
        foreach (Fact fact in factsList)
        {
            //works fine with few amounts to instantiate, but will need to do object pooling for future.
            SingleFactUI singleFactUI = Instantiate(singleFactUIPrefab, contentPanel); 
            singleFactUI.InitializeValues(fact.type);
            singleFactUI.OnSignleFactUIClicked += singleFactUI_OnSignleFactUIClicked;
            SingleFactUIList.Add(singleFactUI);

            singleFactUI.transform.localScale = Vector3.zero;
            singleFactUI.transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        // Wait for the last animation to complete
        yield return new WaitForSeconds(scaleDuration);

        onCompleteCallback?.Invoke();
    }

    private void singleFactUI_OnSignleFactUIClicked(SingleFactUI singleFactUI)
    {
        int index = SingleFactUIList.IndexOf(singleFactUI);
        if (index == -1) return;
        OnFactPopUpRequested?.Invoke(index);
    }

    public void ChangeInteractive(bool interactive)
    {
        if(!interactive)
        {
            factsPanelCanvasGroup.DOFade(inactivePanelOpacity, 0.5f);
        }
        else
        {
            factsPanelCanvasGroup.DOFade(1f, 0.5f);
        }

        factsPanelCanvasGroup.blocksRaycasts = interactive;
        factsPanelCanvasGroup.interactable = interactive;
    }

    private void OnDisable()
    {
        refreshFactsUI.OnRefreshButtonClick -= refreshFactsUI_OnRefreshButtonClick;
    }
}
