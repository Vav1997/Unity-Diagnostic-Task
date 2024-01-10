using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactsController : MonoBehaviour
{
    [SerializeField] private FactsPanelUI factsPanelUI;
    [SerializeField] private SingleFactPopUpUI singleFactPopUpUI;
    [SerializeField] private APIHelper apiHelper;
    private string API_URL = "https://cat-fact.herokuapp.com/facts/random?animal_type=cat,dog,horse&amount=10";
    private List<Fact> currentFactsList = new List<Fact>();
    private bool isRefreshingFacts;

    private void OnEnable()
    {
        // Subsribe to events for handling UI inputs in the controller
        factsPanelUI.OnFactPopUpRequested += factsPanelUI_OnFactPopUpRequested;
        factsPanelUI.OnRefreshFactsRequested += factsPanelUI_OnRefreshFactsRequested;
        singleFactPopUpUI.OnPopUpCloseRequested += singleFactPopUpUI_OnPopUpCloseRequested;
    }

    private void singleFactPopUpUI_OnPopUpCloseRequested(object sender, EventArgs e)
    {
        singleFactPopUpUI.Hide();
        factsPanelUI.ChangeInteractive(true);
    }

    private void Start()
    {
        isRefreshingFacts = true;
        StartCoroutine(APIHelper.Instance.RequestFacts(API_URL, OnFactsReceived));
    }


    private void factsPanelUI_OnRefreshFactsRequested(object sender, EventArgs e)
    {
        if (isRefreshingFacts) return;
        factsPanelUI.StartEraseFactsWithDelay(OnFactsErased);
        isRefreshingFacts = true;
    }

    private void OnFactsErased()
    {
        StartCoroutine(APIHelper.Instance.RequestFacts(API_URL, OnFactsReceived));
    }

    private void factsPanelUI_OnFactPopUpRequested(int index)
    {
        Fact factData = GetFactAt(index);
        if(factData != null)
        {
            factsPanelUI.ChangeInteractive(false);
            singleFactPopUpUI.Show(factData.text);
        }
    }

    private void OnFactsReceived(List<Fact> factsList)
    {
        currentFactsList = factsList;
        factsPanelUI.DrawFacts(factsList, OnFactsDrawFinished);
    }

    private void OnFactsDrawFinished()
    {
        isRefreshingFacts = false;
    }

    public Fact GetFactAt(int index)
    {
        return currentFactsList[index];
    }

}
