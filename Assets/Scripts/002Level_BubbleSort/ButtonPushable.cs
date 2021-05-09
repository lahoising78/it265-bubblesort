using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPushable : Listener
{
    private bool isTouched = false, isPushable = true;
    public bool isPoppable = false;
    private float heightDiff, startYHand, startYButton;
    public Transform buttonUpPosition, buttonDownPosition;
    private Color activeColor = new Color(0f, 0f, 0f, 1f);
    private Color disabledColor = new Color(0f, 0f, 0f, 0.25f);
    //private Color pushedColor = new Color(0f, 0.6f, 0f, 1f);
    private bool isBlinking;

    //collider isnt popping into the right place
    //color should pop into another color once you release not once its down

    private void Start()
    {
        ResetButton(false);
    }

    public void ResetButton(bool active)
    {
        transform.localPosition = buttonUpPosition.localPosition;
        //GetComponent<BoxCollider>().center = Vector3.zero;
        isTouched = false;
        isBlinking = false;

        if (active)
        {
            transform.GetChild(0).GetComponent<Image>().color = activeColor;
            isPushable = true;
        }
        else
        {
            Debug.Log("turn off " + gameObject.name);
            transform.GetChild(0).GetComponent<Image>().color = disabledColor;
            isPushable = false;
        }
    }

    public void StartTutorialBlink()
    {
        isBlinking = true;
        StartCoroutine("Blinking");
    }

    public void EndTutorialBlink()
    {
        isBlinking = false;
    }
    /*
    public override void ResetButtons(bool status)
    {
        ResetButton(status);
    }
    */
    private IEnumerator Blinking()
    {
        float elapsedTime, waitTime = 1f;
        Color startColor, endColor;
        bool isFadingUp = false;

        while(isBlinking)
        {
            elapsedTime = 0;
            if(isFadingUp)
            {
                startColor = activeColor;
                endColor = disabledColor;
            }
            else
            {
                startColor = disabledColor;
                endColor = activeColor;
            }
            
            while (isBlinking && elapsedTime < waitTime)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(startColor, endColor, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            isFadingUp = !isFadingUp;
            yield return null;
        }
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && isPushable) //Hand sphere
        {
            if(other.transform.position.y > buttonDownPosition.position.y)
            {
                isTouched = true;
                heightDiff = other.transform.position.y - transform.position.y;
                startYHand = other.transform.position.y;
                startYButton = transform.position.y;
                //Debug.Log("button is touched, hand is at " + startYHand.ToString() + ", button is at" + startYButton.ToString());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9 && isTouched) //Hand sphere
        {
            //button is below its down position, ie user has pressed the button
            if(transform.position.y < buttonDownPosition.position.y)
            {
                transform.position = buttonDownPosition.position;
                transform.GetChild(0).GetComponent<Image>().color = disabledColor;
                isTouched = false;
                isPushable = false;
                isBlinking = false;
                level.ButtonPushed(gameObject.name);
                //GetComponent<BoxCollider>().center = new Vector3(0f, 1f, 0f);
                //Debug.Log("button is down " + transform.localPosition.z.ToString() + " " + buttonDownPosition.localPosition.z.ToString());
            }
            //button is above its up position, and there are two use cases for this sitation:
            //1. valid action: user has changed mind about pressing button mid-push and is releasing button before executing it
            //2. invalid action: user came from underneath button and pushed it up which is not a defined action
            else if(transform.position.y > buttonUpPosition.position.y)
            {
                transform.position = buttonUpPosition.position;
                //GetComponent<BoxCollider>().center = Vector3.zero;
                //Debug.Log("button is up " + transform.position.y.ToString());
            }
            //button is inbetween the up and down position, ie the user is actively pushing the button down or releasing the button
            else
            {
                transform.position = new Vector3(transform.position.x, startYButton - (startYHand - other.transform.position.y), transform.position.z);
                //GetComponent<BoxCollider>().center = buttonUpPosition.localPosition;
                //Debug.Log("button is intransition" + transform.position.y.ToString());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.name + " exited");
        if (other.gameObject.layer == 9 && isPoppable) //Hand sphere && releasing button after not pushing it all the way down
        {
            transform.position = buttonUpPosition.position;
            //ResetButton(true);
        }
    }
}
