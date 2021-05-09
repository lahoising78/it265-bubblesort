using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortLevel : MonoBehaviour
{ 
    private List<Listener> listeners = new List<Listener>();
    public BubbleSortState currentState;
    public float resetTimer = 2f;

    void Start()
    {
        //SetAppState(BubbleSortState.IntroductionToNextButton);
        SetAppState(currentState);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) SetAppState(BubbleSortState.IntroductionToNextButton);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) SetAppState(BubbleSortState.BeginnerBubbleSortTask01);
    }

    public void RegisterListener(Listener newListener)
    {
        listeners.Add(newListener);
        Debug.Log("Registered " + newListener.gameObject.name);
    }

    private void SetAppState(BubbleSortState tempState)
    {
        currentState = tempState;
        Debug.Log(currentState.ToString());
        foreach (Listener listenerObj in listeners) listenerObj.SetListenerState(tempState);
    }

    public void ButtonPushed(string buttonName)
    {
        switch (currentState)
        {
            case BubbleSortState.IntroductionToNextButton:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToSwapButton);
                break;

            case BubbleSortState.IntroductionToSwapButton:
                if (buttonName == "ButtonSwap")
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    Invoke("SetAppStateToIntroductionToCyclingThroughList", resetTimer);
                }
                break;

            case BubbleSortState.IntroductionToCyclingThroughList:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToFinishingBubbleSort);
                break;

            case BubbleSortState.IntroductionToFinishingBubbleSort:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToElementChecking);
                break;

            case BubbleSortState.IntroductionToElementChecking:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToBestCaseScenario);
                break;

            case BubbleSortState.IntroductionToBestCaseScenario:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToWorstCaseScenario);
                break;

            case BubbleSortState.IntroductionToWorstCaseScenario:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToDuplicateNumbers);
                break;

            case BubbleSortState.IntroductionToDuplicateNumbers:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToThreeElementList01);
                break;

            case BubbleSortState.IntroductionToThreeElementList01:
                if (buttonName == "ButtonNext")
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    SetAppState(BubbleSortState.IntroductionToThreeElementList02);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList02:
                if (buttonName == "ButtonSwap")
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    Invoke("SetAppStateToIntroductionToThreeElementList03", resetTimer);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList03:
                if (buttonName == "ButtonNext")
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    SetAppState(BubbleSortState.IntroductionToThreeElementList04);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList04:
                if (buttonName == "ButtonSwap")
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    Invoke("SetAppStateToIntroductionToThreeElementList05", resetTimer);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList05:
                if (buttonName == "ButtonNext")
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    SetAppState(BubbleSortState.IntroductionToThreeElementList06);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList06:
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToThreeElementList07);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList07:
                {
                    foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                    if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToThreeElementList08);
                }
                break;

            case BubbleSortState.IntroductionToThreeElementList08:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.IntroductionToThreeElementList09);
                break;

            case BubbleSortState.IntroductionToThreeElementList09:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.BeginnerBubbleSortTask01);
                break;

            case BubbleSortState.BeginnerBubbleSortTask01Complete:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.BeginnerBubbleSortTask02);
                break;

            case BubbleSortState.BeginnerBubbleSortTask02Complete:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.BeginnerBubbleSortTask03);
                break;

            case BubbleSortState.BeginnerBubbleSortTask03Complete:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.BeginnerBubbleSortTask04);
                break;

            case BubbleSortState.BeginnerBubbleSortTask04Complete:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.BeginnerBubbleSortTask05);
                break;

            case BubbleSortState.BeginnerBubbleSortTask05Complete:
                if (buttonName == "ButtonNext") SetAppState(BubbleSortState.BeginnerBubbleSortTask06);
                break;

            default:
                Debug.Log("button pushed " + buttonName);
                foreach (Listener listenerObj in listeners) listenerObj.ButtonPushed(buttonName);
                break;
        }
    }

    public void TaskSuccessful()
    {
        foreach (Listener listenerObj in listeners) listenerObj.TaskSuccessful();
        switch (currentState)
        {
            case BubbleSortState.BeginnerBubbleSortTask01:
                SetAppState(BubbleSortState.BeginnerBubbleSortTask01Complete);
                break;

            case BubbleSortState.BeginnerBubbleSortTask02:
                SetAppState(BubbleSortState.BeginnerBubbleSortTask02Complete);
                break;

            case BubbleSortState.BeginnerBubbleSortTask03:
                SetAppState(BubbleSortState.BeginnerBubbleSortTask03Complete);
                break;

            case BubbleSortState.BeginnerBubbleSortTask04:
                SetAppState(BubbleSortState.BeginnerBubbleSortTask04Complete);
                break;

            case BubbleSortState.BeginnerBubbleSortTask05:
                SetAppState(BubbleSortState.BeginnerBubbleSortTask05Complete);
                break;

            case BubbleSortState.BeginnerBubbleSortTask06:
                SetAppState(BubbleSortState.BeginnerBubbleSortTask06Complete);
                break;
        }
    }

    public void TaskUnsuccessful(int hint)
    {
        foreach (Listener listenerObj in listeners) listenerObj.TaskUnsuccessful(hint);
    }

    public void PopulateContainer(int containerPosition, int containerContents)
    {
        foreach (Listener listenerObj in listeners) listenerObj.PopulateContainer(containerPosition, containerContents);
    }

    public void DepopulateContainer(int containerPosition)
    {
        foreach (Listener listenerObj in listeners) listenerObj.DepopulateContainer(containerPosition);
    }

    private void SetAppStateToIntroductionToCyclingThroughList()
    {
        SetAppState(BubbleSortState.IntroductionToCyclingThroughList);
    }

    private void SetAppStateToIntroductionToThreeElementList03()
    {
        SetAppState(BubbleSortState.IntroductionToThreeElementList03);
    }

    private void SetAppStateToIntroductionToThreeElementList05()
    {
        SetAppState(BubbleSortState.IntroductionToThreeElementList05);
    }
}
