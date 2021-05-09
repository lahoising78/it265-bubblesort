using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleSortTray : Listener
{
    public Text passiveInstructionalText, activeInstructionalText, currentStepsText, goalStepsText;
    public Image passiveInstructionalPanel, activeInstructionalPanel;
    private Color blankColor = new Color(0f, 0f, 0f, 0f);
    private float start, end, time;
    [SerializeField]
    private ButtonPushable buttonSwap, buttonNext;
    private IEnumerator coroutine;
    private string passiveInstruction, activeInstruction;
    private Vector3 trayEndPosition;
    public UIControls uiControls;
    private bool taskSuccessful;
    private int currentStepsTaken, goalSteps;

    private void Awake()
    {
        base.Awake();
        currentStepsText.text = "";
        goalStepsText.text = "";
    }

    private IEnumerator TrayTransition(Vector3 startPosition, Vector3 endPosition, float moveTime = 4f, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPosition;
        yield return null;
    }

    private IEnumerator TextTransition(Text textElement, Color startColor, Color endColor, string endText, float moveTime = 4f, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        if (endColor.a == 1) textElement.text = endText;
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            textElement.color = Color.Lerp(startColor, endColor, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textElement.color = endColor;
        yield return null;
    }

    private IEnumerator ColorTransition(Image imageElement, Color startColor, Color endColor, float moveTime = 4f, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            imageElement.color = Color.Lerp(startColor, endColor, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageElement.color = endColor;
        yield return null;
    }

    private void ResetTray(float transitionTime = 0)
    {
        coroutine = TextTransition(passiveInstructionalText, passiveInstructionalText.color, blankColor, "", transitionTime, 0f);
        StartCoroutine(coroutine);
        coroutine = TextTransition(activeInstructionalText, passiveInstructionalText.color, blankColor, "", transitionTime, 0f);
        StartCoroutine(coroutine);

        buttonSwap.ResetButton(false);
        buttonNext.ResetButton(false);
    }

    private void TurnOnButtonNext()
    {
        buttonNext.ResetButton(true);
        buttonNext.StartTutorialBlink();
    }

    private void TurnOffButtonNext()
    {
        buttonNext.ResetButton(false);
    }

    private void TurnOnButtonSwap()
    {
        buttonSwap.ResetButton(true);
        buttonSwap.StartTutorialBlink();
    }

    private void TurnOffButtonSwap()
    {
        buttonSwap.ResetButton(false);
    }

    private void TurnOnButtons()
    {
        if(taskSuccessful)
        {
            Invoke("TurnOnButtonNext", 3f);
            //buttonNext.ResetButton(true);
            taskSuccessful = false;
        }
        else
        {
            buttonNext.ResetButton(true);
            buttonSwap.ResetButton(true);
        }
    }

    public override void TaskUnsuccessful(int hint)
    {
        coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer / 2f, 0f);
        StartCoroutine(coroutine);
        passiveInstruction = "List is not in order. You might want to check around container " + hint.ToString() + ".";
        coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer / 2f);
        StartCoroutine(coroutine);
        coroutine = ColorTransition(passiveInstructionalPanel, Color.white, Color.red, level.resetTimer, level.resetTimer / 2f);
        StartCoroutine(coroutine);
        coroutine = ColorTransition(passiveInstructionalPanel, Color.red, Color.white, level.resetTimer, level.resetTimer * 3f);
        StartCoroutine(coroutine);
    }

    public void UpdateTray(int containerPosition, int uiSize)
    {
        if(containerPosition > 0) trayEndPosition = new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y, transform.localPosition.z);
        else trayEndPosition = new Vector3(-(uiSize * 0.25f) + 0.5f, transform.localPosition.y, transform.localPosition.z);
        Debug.Log("Tray is moving to containerPosition " + containerPosition.ToString() + ", with a UI of size " + uiSize.ToString() + ", to ending position " + trayEndPosition.ToString());
        coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
        StartCoroutine(coroutine);
    }

    public override void TaskSuccessful()
    {
        coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer / 2f, 0f);
        StartCoroutine(coroutine);
        coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer / 2f, 0f);
        StartCoroutine(coroutine);
        int taskScore = currentStepsTaken - goalSteps;
        passiveInstruction = "You successfully sorted the list with " + taskScore.ToString() + " step(s) over the goal.";
        if (level.currentState == BubbleSortState.BeginnerBubbleSortTask06Complete) activeInstruction = "You completed the tutorial! Please remove the headset.";
        else if (taskScore == 0) activeInstruction = "That is a perfect score, great job! Press next to proceed for next task.";
        else if (taskScore <= Mathf.CeilToInt(0.15f * taskScore)) activeInstruction = "That is a close score, good job! Press next to proceed for next task.";
        else activeInstruction = "That is a good try, let's try to get a better score in the next task. Press next to proceed.";
        coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer / 2f);
        StartCoroutine(coroutine);
        coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 1.5f);
        StartCoroutine(coroutine);
        coroutine = ColorTransition(passiveInstructionalPanel, Color.white, Color.green, level.resetTimer, level.resetTimer / 2f);
        StartCoroutine(coroutine);
        coroutine = ColorTransition(passiveInstructionalPanel, Color.green, Color.white, level.resetTimer, level.resetTimer * 2f);
        StartCoroutine(coroutine);
        coroutine = ColorTransition(activeInstructionalPanel, Color.white, Color.green, level.resetTimer, level.resetTimer * 1.5f);
        StartCoroutine(coroutine);
        coroutine = ColorTransition(activeInstructionalPanel, Color.green, Color.white, level.resetTimer, level.resetTimer * 3f);
        StartCoroutine(coroutine);
        taskSuccessful = true;
    }

    public override void ButtonPushed(string buttonName)
    {
        switch (level.currentState)
        {
            case BubbleSortState.BeginnerBubbleSortTask01:
            case BubbleSortState.BeginnerBubbleSortTask02:
            case BubbleSortState.BeginnerBubbleSortTask03:
            case BubbleSortState.BeginnerBubbleSortTask04:
            case BubbleSortState.BeginnerBubbleSortTask05:
            case BubbleSortState.BeginnerBubbleSortTask06:
                //Debug.Log(buttonName + " has been pushed. This is " + gameObject.name);
                if (buttonName == "ButtonSwap") buttonNext.ResetButton(false);
                else
                {
                    currentStepsTaken++;
                    currentStepsText.text = currentStepsTaken.ToString();
                    buttonSwap.ResetButton(false);
                }
                Invoke("TurnOnButtons", level.resetTimer);
                break;

            case BubbleSortState.IntroductionToThreeElementList01:
            case BubbleSortState.IntroductionToThreeElementList02:
            case BubbleSortState.IntroductionToThreeElementList03:
            case BubbleSortState.IntroductionToThreeElementList04:
            case BubbleSortState.IntroductionToThreeElementList05:
            case BubbleSortState.IntroductionToThreeElementList06:
            case BubbleSortState.IntroductionToThreeElementList07:
            case BubbleSortState.IntroductionToThreeElementList08:
            case BubbleSortState.IntroductionToThreeElementList09:
                if (buttonName == "ButtonNext")
                {
                    currentStepsTaken++;
                    currentStepsText.text = currentStepsTaken.ToString();
                }
                break;
        }
        
    }

    public override void SetListenerState(BubbleSortState currentState)
    {
        switch (currentState)
        {
            case BubbleSortState.IntroductionToNextButton:
                //ResetTray();
                uiControls.SetUISize(0);
                passiveInstructionalText.text = "";
                activeInstructionalText.text = "";
                buttonSwap.ResetButton(false);
                buttonNext.ResetButton(false);
                passiveInstruction = "Welcome to the Bubble Sort trainer. Let's learn to use the controls.";
                activeInstruction = "Find the blinking button. It is the \"next\" button. Press it to continue.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                Invoke("TurnOnButtonNext", level.resetTimer * 2f);
                Debug.Log(gameObject.name + " set to " + currentState.ToString());
                break;

            case BubbleSortState.IntroductionToSwapButton:
                //ResetTray();
                uiControls.SetUISize(2);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "In front of you are two containers that each contain a number of objects.";
                activeInstruction = "We need to place them into ascending order. Press the blinking button to swap them.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonSwap", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToCyclingThroughList:
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Notice the numbers inbetween the buttons help you keep track of the order.";
                activeInstruction = "These two containers are now in correct ascending order. Press next button to finish the sort.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonSwap", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToFinishingBubbleSort:
                uiControls.SetUISize(0);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Once we cycle through the list without having to swap any containers, the sort is complete.";
                activeInstruction = "Press the next button to learn how to keep take of the number of steps taken in the sort.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToElementChecking:
                coroutine = TextTransition(currentStepsText, blankColor, Color.black, "0", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "In front of you is a counter that tracks how many steps it takes you to bubble sort a list.";
                activeInstruction = "A step is when you compare two elements to decide to swap or not, then move to next two. Press next.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToBestCaseScenario:
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "The best case scenario for bubble sort is when the list is already in ascending order.";
                activeInstruction = "This means you cycle through the list once and you are done. This takes n amount of steps. Press next.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToWorstCaseScenario:
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Worst case is when the list is in descending order. Then you have to cycle through the list n^2 times.";
                activeInstruction = "Try to minimize the steps you do to complete a bubble sort. Press next to learn about duplicate numbers.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToDuplicateNumbers:
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "When you come across elements that have the same number, you do not need to swap the elements.";
                activeInstruction = "Swapping duplicate numbers has no impact to the order of the list. Press next to learn to sort a list of three.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList01:
                coroutine = TextTransition(currentStepsText, blankColor, Color.black, "0", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                currentStepsTaken = 0;
                uiControls.SetUISize(3);
                trayEndPosition = new Vector3(-0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "In front of you are now three containers. Lets learn to sort a list of three containers.";
                activeInstruction = "Focus on two containers at a time. These two are in order and dont need to swap, press next.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList02:
                trayEndPosition = new Vector3(0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Notice we moved only one container to the right, and now we examine these two containers.";
                activeInstruction = "These two containers are not in ascending order. Press swap to place them in correct order.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonSwap", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList03:
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Notice that these two are now in order but the previous two are now not in order.";
                activeInstruction = "We need to return to the beginning of the list to swap them. Press next to move to beginning.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonSwap", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList04:
                trayEndPosition = new Vector3(-0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Notice that we may need to cycle through the list to continue swapping to get all in order.";
                activeInstruction = "Let's swap these two containers to place them into ascending order. Press swap.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonSwap", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList05:
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "These two containers are now in order. Notice the entire list is now in ascending order.";
                activeInstruction = "But we only focus on two containers at a time. Let's press next to continue checking the list.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonSwap", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;
                
            case BubbleSortState.IntroductionToThreeElementList06:
                trayEndPosition = new Vector3(0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "These two containers are also in ascending order. There is no need to swap them.";
                activeInstruction = "Since there is no need to swap, let's press the next button to cycle back to beginning of list.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList07:
                trayEndPosition = new Vector3(-0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "The bubble sort algorithm does not finish until we go through the entire list without swapping.";
                activeInstruction = "That way we know for sure that the list is sorted. Press next since these two are in order.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList08:
                trayEndPosition = new Vector3(0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "We are at the end of the list and have not swapped previous elements this cycle through.";
                activeInstruction = "These two are also in ascending order. Press next to complete the final cycle through the list.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.IntroductionToThreeElementList09:
                uiControls.SetUISize(0);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Congratulations on learning the basics of a bubble sort. Now let's do a series of exercises.";
                activeInstruction = "Once you complete these exercises, we will move to freeform bubble sort. Press next to begin.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtonNext", level.resetTimer * 3f);
                break;

            case BubbleSortState.BeginnerBubbleSortTask01:
                //stepsTitle.text = "Steps";
                currentStepsTaken = 0;
                currentStepsText.text = currentStepsTaken.ToString();
                goalSteps = 6;
                goalStepsText.text = goalSteps.ToString();
                uiControls.SetUISize(3);
                trayEndPosition = new Vector3(-0.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Here is a list of 3 containers. Sort them in ascending order. Focus on two containers at a time.";
                activeInstruction = "Remember you must cycle through the list without a swap to complete the bubble sort correctly.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtons", level.resetTimer * 3f);
                break;

            case BubbleSortState.BeginnerBubbleSortTask02:
                //stepsTitle.text = "Steps";
                currentStepsTaken = 0;
                currentStepsText.text = currentStepsTaken.ToString();
                goalSteps = 9;
                goalStepsText.text = goalSteps.ToString();
                uiControls.SetUISize(4);
                trayEndPosition = new Vector3(-0.5f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Here is a list of 4 containers. Short them in ascending order. Focus on two containers at a time.";
                activeInstruction = "Remember you must cycle through the list without a swap to complete the bubble sort correctly.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtons", level.resetTimer * 3f);
                break;

            case BubbleSortState.BeginnerBubbleSortTask03:
                //stepsTitle.text = "Steps";
                currentStepsTaken = 0;
                currentStepsText.text = currentStepsTaken.ToString();
                goalSteps = 12;
                goalStepsText.text = goalSteps.ToString();
                uiControls.SetUISize(5);
                trayEndPosition = new Vector3(-0.75f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Here is a list of 5 containers. Short them in ascending order. Focus on two containers at a time.";
                activeInstruction = "Remember you must cycle through the list without a swap to complete the bubble sort correctly.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtons", level.resetTimer * 3f);
                break;

            case BubbleSortState.BeginnerBubbleSortTask04:
                //stepsTitle.text = "Steps";
                currentStepsTaken = 0;
                currentStepsText.text = currentStepsTaken.ToString();
                goalSteps = 20;
                goalStepsText.text = goalSteps.ToString();
                uiControls.SetUISize(6);
                trayEndPosition = new Vector3(-1f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Here is a list of 6 containers. Short them in ascending order. Focus on two containers at a time.";
                activeInstruction = "Remember you must cycle through the list without a swap to complete the bubble sort correctly.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtons", level.resetTimer * 3f);
                break;

            case BubbleSortState.BeginnerBubbleSortTask05:
                //stepsTitle.text = "Steps";
                currentStepsTaken = 0;
                currentStepsText.text = currentStepsTaken.ToString();
                goalSteps = 30;
                goalStepsText.text = goalSteps.ToString();
                uiControls.SetUISize(7);
                trayEndPosition = new Vector3(-1.25f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Here is a list of 7 containers. Short them in ascending order. Focus on two containers at a time.";
                activeInstruction = "Remember you must cycle through the list without a swap to complete the bubble sort correctly.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtons", level.resetTimer * 3f);
                break;

            case BubbleSortState.BeginnerBubbleSortTask06:
                //stepsTitle.text = "Steps";
                currentStepsTaken = 0;
                currentStepsText.text = currentStepsTaken.ToString();
                goalSteps = 35;
                goalStepsText.text = goalSteps.ToString();
                uiControls.SetUISize(8);
                trayEndPosition = new Vector3(-1.5f, transform.localPosition.y, transform.localPosition.z);
                coroutine = TrayTransition(transform.localPosition, trayEndPosition, level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(passiveInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, Color.black, blankColor, "", level.resetTimer, 0f);
                StartCoroutine(coroutine);
                passiveInstruction = "Here is a list of 8 containers. Short them in ascending order. Focus on two containers at a time.";
                activeInstruction = "Remember you must cycle through the list without a swap to complete the bubble sort correctly.";
                coroutine = TextTransition(passiveInstructionalText, blankColor, Color.black, passiveInstruction, level.resetTimer, level.resetTimer);
                StartCoroutine(coroutine);
                coroutine = TextTransition(activeInstructionalText, blankColor, Color.black, activeInstruction, level.resetTimer, level.resetTimer * 2f);
                StartCoroutine(coroutine);
                Invoke("TurnOffButtonNext", level.resetTimer);
                Invoke("TurnOnButtons", level.resetTimer * 3f);
                break;

            default:
                ResetTray();
                break;
        }
    }

    private void TurnOnActiveInstruction()
    {
        activeInstructionalText.color = Color.black;
        //later on fade up and down text
        //StartCoroutine("TransitionEffect");
    }

    private IEnumerator TransitionEffect()
    {
        return null;
    }
}
