using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Listener : MonoBehaviour
{
    static protected BubbleSortLevel level;

    public virtual void SetListenerState(BubbleSortState currentState) { }

    public virtual void ButtonPushed(string buttonName) { }

    public virtual void PopulateContainer(int containerPosition, int containerContents) { }

    public virtual void DepopulateContainer(int containerPosition) { }

    public virtual void ResetButtons(bool status) { }

    public virtual void TaskUnsuccessful(int hint) { }

    public virtual void TaskSuccessful() { }

    protected void Awake()
    {
        if(level == null) level = GameObject.FindGameObjectWithTag("GameController").GetComponent<BubbleSortLevel>();
        level.RegisterListener(this);
    }

}

