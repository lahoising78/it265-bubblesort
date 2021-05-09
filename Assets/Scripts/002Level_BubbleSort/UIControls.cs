using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : Listener {

    public Transform buttonSwap, buttonNext;
    public RectTransform rectTranBkgrd, rectTranHighlightLeft, rectTranHighlightRight, rotationCenter;
    public Text[] textContainerAmount;
    private IEnumerator coroutine;
    private const float textOffset = 26f, highlightWidth = 50f, 
        textBkgrdWidth = 50f, textBkgrdWidthOffset = 80f, textBkgrdHeight = 70f,
        buttonLocalPositionXOffset = 80f, buttonLocalPositionY = -206f, buttonLocalPositionZ = -12f;
    private int containerOnLeftSideOfTray;
    private Color highlightColorOn = new Color(0f, 0.25f, 0.75f, 0.35f), highlightColorOff = new Color(0f, 0.25f, 0.75f, 0f);

    private int uiSize;
    public BubbleSortTray tray;

    public override void PopulateContainer(int containerPosition, int containerContents)
    {
        coroutine = TextTransition(textContainerAmount[containerPosition], Color.clear, Color.black, containerContents.ToString(), level.resetTimer + 2f, 0f);
        StartCoroutine(coroutine);
    }

    public override void DepopulateContainer(int containerPosition)
    {
        coroutine = TextTransition(textContainerAmount[containerPosition], Color.black, Color.clear, "", level.resetTimer, 0f);
        StartCoroutine(coroutine);
    }

    //pick up here
    //method for ContainerManager to call when swap and next buttons are pressed

    public override void ButtonPushed(string buttonName)
    {
        if (buttonName == "ButtonSwap")
        {
            
        }

        if (buttonName == "ButtonNext")
        {
            
        }
    }

    public void SetUISize(int size)
    {
        uiSize = size;
        switch(size)
        {
            case 0:
            case 1:
            default:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 3f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOff;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOff;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-120f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(120f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 2:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 3f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-1f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(1f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-120f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(120f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 3:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 3f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-2f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(0f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-2f * textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(0f * textOffset, 0f, 0f);
                textContainerAmount[2].rectTransform.localPosition = new Vector3(2f * textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-120f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(120f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 4:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 4f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-3f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(-1f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-3f * textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(-1f * textOffset, 0f, 0f);
                textContainerAmount[2].rectTransform.localPosition = new Vector3(1f * textOffset, 0f, 0f);
                textContainerAmount[3].rectTransform.localPosition = new Vector3(3f * textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-160f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(160f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 5:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 5f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-4f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(-2f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-4f * textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(-2f * textOffset, 0f, 0f);
                textContainerAmount[2].rectTransform.localPosition = new Vector3(0f * textOffset, 0f, 0f);
                textContainerAmount[3].rectTransform.localPosition = new Vector3(2f * textOffset, 0f, 0f);
                textContainerAmount[4].rectTransform.localPosition = new Vector3(4f * textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-180f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(180f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 6:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 6f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-5f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(-3f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-5f * textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(-3f * textOffset, 0f, 0f);
                textContainerAmount[2].rectTransform.localPosition = new Vector3(-1f * textOffset, 0f, 0f);
                textContainerAmount[3].rectTransform.localPosition = new Vector3(1f * textOffset, 0f, 0f);
                textContainerAmount[4].rectTransform.localPosition = new Vector3(3f * textOffset, 0f, 0f);
                textContainerAmount[5].rectTransform.localPosition = new Vector3(5f * textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-200f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(200f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 7:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 7f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-6f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(-4f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-6f * textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(-4f * textOffset, 0f, 0f);
                textContainerAmount[2].rectTransform.localPosition = new Vector3(-2f * textOffset, 0f, 0f);
                textContainerAmount[3].rectTransform.localPosition = new Vector3(0f * textOffset, 0f, 0f);
                textContainerAmount[4].rectTransform.localPosition = new Vector3(2f * textOffset, 0f, 0f);
                textContainerAmount[5].rectTransform.localPosition = new Vector3(4f * textOffset, 0f, 0f);
                textContainerAmount[6].rectTransform.localPosition = new Vector3(6f * textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-230f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(230f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
            case 8:
                rectTranBkgrd.sizeDelta = new Vector2(textBkgrdWidth * 8f + textBkgrdWidthOffset, textBkgrdHeight);
                rectTranHighlightLeft.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightLeft.localPosition = new Vector3(-7f * textOffset, 0f, 0f);
                rectTranHighlightLeft.GetComponent<Image>().color = highlightColorOn;
                rectTranHighlightRight.sizeDelta = new Vector2(highlightWidth, textBkgrdHeight);
                rectTranHighlightRight.localPosition = new Vector3(-5f * textOffset, 0f, 0f);
                rectTranHighlightRight.GetComponent<Image>().color = highlightColorOn;
                textContainerAmount[0].rectTransform.localPosition = new Vector3(-7f * textOffset, 0f, 0f);
                textContainerAmount[1].rectTransform.localPosition = new Vector3(-5f * textOffset, 0f, 0f);
                textContainerAmount[2].rectTransform.localPosition = new Vector3(-3f * textOffset, 0f, 0f);
                textContainerAmount[3].rectTransform.localPosition = new Vector3(-1f * textOffset, 0f, 0f);
                textContainerAmount[4].rectTransform.localPosition = new Vector3(1f * textOffset, 0f, 0f);
                textContainerAmount[5].rectTransform.localPosition = new Vector3(3f * textOffset, 0f, 0f);
                textContainerAmount[6].rectTransform.localPosition = new Vector3(5f * textOffset, 0f, 0f);
                textContainerAmount[7].rectTransform.localPosition = new Vector3(7f * textOffset, 0f, 0f);
                buttonSwap.localPosition = new Vector3(-260f, buttonLocalPositionY, buttonLocalPositionZ);
                buttonNext.localPosition = new Vector3(260f, buttonLocalPositionY, buttonLocalPositionZ);
                foreach (Text textComponent in textContainerAmount) textComponent.text = "";
                break;
        }
    }

    public void UpdateHighlights(int incomingInt)
    {
        containerOnLeftSideOfTray = incomingInt;
        coroutine = MoveHighlights(level.resetTimer, 0f);
        StartCoroutine(coroutine);
        tray.UpdateTray(incomingInt, uiSize);
    }

    public void SwapContainer(int leftSideContainer, int leftAmount, int rightAmount)
    {
        containerOnLeftSideOfTray = leftSideContainer;
        /*
        string oldLeftText = textContainerAmount[containerOnLeftSideOfTray].text;
        string oldRightText = textContainerAmount[containerOnLeftSideOfTray + 1].text;
        coroutine = TextTransition(textContainerAmount[containerOnLeftSideOfTray], Color.black, Color.clear, "", level.resetTimer / 2f, 0f);
        StartCoroutine(coroutine);
        coroutine = TextTransition(textContainerAmount[containerOnLeftSideOfTray + 1], Color.black, Color.clear, "", level.resetTimer / 2f, 0f);
        StartCoroutine(coroutine);
        coroutine = TextTransition(textContainerAmount[containerOnLeftSideOfTray], Color.clear, Color.black, oldRightText, level.resetTimer / 2f, level.resetTimer);
        StartCoroutine(coroutine);
        coroutine = TextTransition(textContainerAmount[containerOnLeftSideOfTray + 1], Color.clear, Color.black, oldLeftText, level.resetTimer / 2f, level.resetTimer);
        */    
        StartCoroutine(coroutine);
        StartCoroutine("SwapContainerText");
    }

    private IEnumerator SwapContainerText()
    {
        float elapsedTime = 0, moveTime = level.resetTimer;
        Text tempTextComponent;
        //Debug.Log(containerOnLeftSideOfTray);
        Vector3 leftPosition = textContainerAmount[containerOnLeftSideOfTray].transform.position;
        Vector3 rightPosition = textContainerAmount[containerOnLeftSideOfTray + 1].transform.position;
        //trying to control the normal of the arc that right container slerps across
        rotationCenter.position = (rightPosition + leftPosition) * 0.5f;
        Quaternion startRotation = Quaternion.identity;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, 179.0f);
        Quaternion endRotationReverse = Quaternion.Euler(0f, 0f, -179.0f);
        textContainerAmount[containerOnLeftSideOfTray + 1].transform.parent = rotationCenter;

        while (elapsedTime < moveTime)
        {
            textContainerAmount[containerOnLeftSideOfTray].transform.position = Vector3.Lerp(leftPosition, rightPosition, (elapsedTime / moveTime));
            //textContainerAmount[containerOnLeftSideOfTray + 1].transform.position = Vector3.Lerp(rightPosition, leftPosition, (elapsedTime / moveTime));
            //rotation around a center point has issues with text, simplifying for now
            rotationCenter.localRotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / moveTime));
            textContainerAmount[containerOnLeftSideOfTray + 1].transform.localRotation = Quaternion.Slerp(startRotation, endRotationReverse, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textContainerAmount[containerOnLeftSideOfTray].transform.position = rightPosition;
        textContainerAmount[containerOnLeftSideOfTray + 1].transform.position = leftPosition;
        textContainerAmount[containerOnLeftSideOfTray + 1].transform.parent = rotationCenter.parent.transform;
        tempTextComponent = textContainerAmount[containerOnLeftSideOfTray];
        textContainerAmount[containerOnLeftSideOfTray] = textContainerAmount[containerOnLeftSideOfTray + 1];
        textContainerAmount[containerOnLeftSideOfTray + 1] = tempTextComponent;
        rotationCenter.localRotation = Quaternion.identity;
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

    private IEnumerator MoveHighlights(float moveTime = 4f, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        float elapsedTime = 0;
        Vector3 leftStart = rectTranHighlightLeft.localPosition;
        Vector3 rightStart = rectTranHighlightRight.localPosition;
        Vector3 leftEnd = textContainerAmount[containerOnLeftSideOfTray].rectTransform.localPosition;
        Vector3 rightEnd = textContainerAmount[containerOnLeftSideOfTray + 1].rectTransform.localPosition;

        while (elapsedTime < moveTime)
        {
            rectTranHighlightLeft.localPosition = Vector3.Lerp(leftStart, leftEnd, (elapsedTime / moveTime));
            rectTranHighlightRight.localPosition = Vector3.Lerp(rightStart, rightEnd, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTranHighlightLeft.localPosition = leftEnd;
        rectTranHighlightRight.localPosition = rightEnd;
        yield return null;
    }
}
