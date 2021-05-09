using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : Listener
{
    [SerializeField]
    private GameObject[] containers = new GameObject[9];
    private int[] containerContents = new int[9];
    private int containerOnLeftSideOfTray, lastContainerToCheck;

    private IEnumerator coroutine;
    private Vector3 sphereLocation;
    public GameObject sphere;
    private GameObject primitive;
    public Transform rotationCenter;

    public UIControls uiControls;

    private bool trackNextButton, atEndOfList, swapButtonWasPressedThisCycle;

    private void Awake()
    {
        base.Awake();
        for(int i = 0; i < containers.Length; i++)
        {
            containers[i] = transform.GetChild(i).gameObject;
            //Debug.Log(i.ToString() + " is " + containers[i].name);
            containerContents[i] = 0;
        }
    }

    public override void ButtonPushed(string buttonName)
    {
        if(buttonName == "ButtonSwap")
        {
            if (containerOnLeftSideOfTray >= 0)
            {
                swapButtonWasPressedThisCycle = true;
                uiControls.SwapContainer(containerOnLeftSideOfTray, containerContents[containerOnLeftSideOfTray], containerContents[containerOnLeftSideOfTray + 1]);
                StartCoroutine("SwapContainer");
            }
        }

        if (buttonName == "ButtonNext")
        {
            if (containers[containerOnLeftSideOfTray + 2].activeSelf == false) atEndOfList = true;
            /*
            if (containerOnLeftSideOfTray == lastContainerToCheck && containerContents[containerOnLeftSideOfTray] <= containerContents[containerOnLeftSideOfTray + 1])
            {
                lastContainerToCheck--;
                atEndOfList = true;
            }
            */
            Debug.Log("listEnd " + atEndOfList.ToString() + ", swapButtonPressedThisCycle " + swapButtonWasPressedThisCycle.ToString());

            if (atEndOfList && !swapButtonWasPressedThisCycle)
            {
                //atEndOfList = false;
                int listCheck = ListIsInOrder();
                if (listCheck == 0) level.TaskSuccessful();
                else if (listCheck > 0)
                {
                    level.TaskUnsuccessful(listCheck);
                    UpdateContainerAndUiPositions();
                }
                else Debug.Log("ListIsInOrder sent back -1 which should not happen, bad logic somewhere.");
            }
            else UpdateContainerAndUiPositions();
        }
    }

    private void UpdateContainerAndUiPositions()
    {
        //this method of checking if tray is at end of currently active container list means that there
        //always needs to be an inactive container gameobject at the end of the list, even when we are
        //at the maximum active containers, basically always have one container at the end of array thats turned off
        if (atEndOfList)
        {
            containerOnLeftSideOfTray = 0;
            atEndOfList = false;
            swapButtonWasPressedThisCycle = false;
        }
        else
        {
            containerOnLeftSideOfTray++;
        }
        //Debug.Log(containerOnLeftSideOfTray.ToString());
        uiControls.UpdateHighlights(containerOnLeftSideOfTray);
    }

    private int ListIsInOrder()
    {
        for (int i = 1; i < containerContents.Length; i++)
        {
            //Debug.Log("Container " + containers[i-1] + " has " + containerContents[i-1].ToString());
            //Debug.Log("Container " + containers[i] +" has " + containerContents[i].ToString());
            //Debug.Log("Container " + containers[i+1] + " has " + containerContents[i+1].ToString());
            //means have to leave the last containerContents always unused and as 0
            if (containerContents[i] == 0) return 0;
            if (containerContents[i - 1] > containerContents[i]) return i;
        }
        return -1; //this should never get sent, just a backup
    }

    private void CleanupContainers()
    {
        ResetContainers(0);
    }

    public override void PopulateContainer(int containerPosition, int content) //double data recorded issue
    {
        for (int i = 0; i < containerContents[containerPosition]; i++)
        {
            sphereLocation = new Vector3(Random.Range(containers[containerPosition].transform.position.x - 0.05f, containers[containerPosition].transform.position.x + 0.05f),
                Random.Range(containers[containerPosition].transform.position.y - 0.1f, containers[containerPosition].transform.position.y + 0.1f),
                Random.Range(containers[containerPosition].transform.position.z - 0.05f, containers[containerPosition].transform.position.z + 0.05f));
            primitive = Instantiate(sphere);
            primitive.transform.position = sphereLocation;
            primitive.transform.parent = containers[containerPosition].transform;
        }
    }

    public override void DepopulateContainer(int containerPosition)
    {
        foreach (Transform child in containers[containerPosition].transform) Destroy(child.gameObject);
        containerContents[containerPosition] = 0;
    }

    private void ResetContainers(int neededAmount = 0)
    {
        float yHeightLow = -0.35f, moveTime = level.resetTimer;
        
        switch(neededAmount)
        {
            case 0:
                for (int i = 0; i < containers.Length; i++)
                {
                    containers[i].transform.localPosition = Vector3.zero;
                    level.DepopulateContainer(i);
                    containers[i].SetActive(false);
                }
                containerOnLeftSideOfTray = -1;
                break;

            case 2:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-0.25f, yHeightLow, 0f), new Vector3(-0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(0.25f, yHeightLow, 0f), new Vector3(0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 0;
                break;

            case -2:
                coroutine = MoveContainer(containers[0], new Vector3(-0.25f, 0f, 0f), new Vector3(-0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(0.25f, 0f, 0f), new Vector3(0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            case 3:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-0.5f, yHeightLow, 0f), new Vector3(-0.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(0f, yHeightLow, 0f), new Vector3(0f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[2].SetActive(true);
                level.PopulateContainer(2, containerContents[2]);
                coroutine = MoveContainer(containers[2], new Vector3(0.5f, yHeightLow, 0f), new Vector3(0.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 1;
                break;

            case -3:
                coroutine = MoveContainer(containers[0], new Vector3(-0.5f, 0f, 0f), new Vector3(-0.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(0f, 0f, 0f), new Vector3(0f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[2], new Vector3(0.5f, 0f, 0f), new Vector3(0.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            case 4:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-0.75f, yHeightLow, 0f), new Vector3(-0.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(-0.25f, yHeightLow, 0f), new Vector3(-0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[2].SetActive(true);
                level.PopulateContainer(2, containerContents[2]);
                coroutine = MoveContainer(containers[2], new Vector3(0.25f, yHeightLow, 0f), new Vector3(0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[3].SetActive(true);
                level.PopulateContainer(3, containerContents[3]);
                coroutine = MoveContainer(containers[3], new Vector3(0.75f, yHeightLow, 0f), new Vector3(0.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 2;
                break;

            case -4:
                coroutine = MoveContainer(containers[0], new Vector3(-0.75f, 0f, 0f), new Vector3(-0.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(-0.25f, 0f, 0f), new Vector3(-0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[2], new Vector3(0.25f, 0f, 0f), new Vector3(0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[3], new Vector3(0.75f, 0f, 0f), new Vector3(0.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            case 5:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-1f, yHeightLow, 0f), new Vector3(-1f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(-0.5f, yHeightLow, 0f), new Vector3(-0.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[2].SetActive(true);
                level.PopulateContainer(2, containerContents[2]);
                coroutine = MoveContainer(containers[2], new Vector3(0f, yHeightLow, 0f), new Vector3(0f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[3].SetActive(true);
                level.PopulateContainer(3, containerContents[3]);
                coroutine = MoveContainer(containers[3], new Vector3(0.5f, yHeightLow, 0f), new Vector3(0.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[4].SetActive(true);
                level.PopulateContainer(4, containerContents[4]);
                coroutine = MoveContainer(containers[4], new Vector3(1f, yHeightLow, 0f), new Vector3(1f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 3;
                break;

            case -5:
                coroutine = MoveContainer(containers[0], new Vector3(-1f, 0f, 0f), new Vector3(-1f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(-0.5f, 0f, 0f), new Vector3(-0.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[2], new Vector3(0f, 0f, 0f), new Vector3(0f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[3], new Vector3(0.5f, 0f, 0f), new Vector3(0.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[4], new Vector3(1f, 0f, 0f), new Vector3(1f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            case 6:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-1.25f, yHeightLow, 0f), new Vector3(-1.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(-0.75f, yHeightLow, 0f), new Vector3(-0.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[2].SetActive(true);
                level.PopulateContainer(2, containerContents[2]);
                coroutine = MoveContainer(containers[2], new Vector3(-0.25f, yHeightLow, 0f), new Vector3(-0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[3].SetActive(true);
                level.PopulateContainer(3, containerContents[3]);
                coroutine = MoveContainer(containers[3], new Vector3(0.25f, yHeightLow, 0f), new Vector3(0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[4].SetActive(true);
                level.PopulateContainer(4, containerContents[4]);
                coroutine = MoveContainer(containers[4], new Vector3(0.75f, yHeightLow, 0f), new Vector3(0.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[5].SetActive(true);
                level.PopulateContainer(5, containerContents[5]);
                coroutine = MoveContainer(containers[5], new Vector3(1.25f, yHeightLow, 0f), new Vector3(1.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 4;
                break;

            case -6:
                coroutine = MoveContainer(containers[0], new Vector3(-1.25f, 0f, 0f), new Vector3(-1.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(-0.75f, 0f, 0f), new Vector3(-0.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[2], new Vector3(-0.25f, 0f, 0f), new Vector3(-0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[3], new Vector3(0.25f, 0f, 0f), new Vector3(0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[4], new Vector3(0.75f, 0f, 0f), new Vector3(0.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[5], new Vector3(1.25f, 0f, 0f), new Vector3(1.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            case 7:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-1.5f, yHeightLow, 0f), new Vector3(-1.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(-1f, yHeightLow, 0f), new Vector3(-1f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[2].SetActive(true);
                level.PopulateContainer(2, containerContents[2]);
                coroutine = MoveContainer(containers[2], new Vector3(-0.5f, yHeightLow, 0f), new Vector3(-0.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[3].SetActive(true);
                level.PopulateContainer(3, containerContents[3]);
                coroutine = MoveContainer(containers[3], new Vector3(0f, yHeightLow, 0f), new Vector3(0f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[4].SetActive(true);
                level.PopulateContainer(4, containerContents[4]);
                coroutine = MoveContainer(containers[4], new Vector3(0.5f, yHeightLow, 0f), new Vector3(0.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[5].SetActive(true);
                level.PopulateContainer(5, containerContents[5]);
                coroutine = MoveContainer(containers[5], new Vector3(1f, yHeightLow, 0f), new Vector3(1f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[6].SetActive(true);
                level.PopulateContainer(6, containerContents[6]);
                coroutine = MoveContainer(containers[6], new Vector3(1.5f, yHeightLow, 0f), new Vector3(1.5f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 5;
                break;

            case -7:
                coroutine = MoveContainer(containers[0], new Vector3(-1.5f, 0f, 0f), new Vector3(-1.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(-1f, 0f, 0f), new Vector3(-1f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[2], new Vector3(-0.5f, 0f, 0f), new Vector3(-0.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[3], new Vector3(0f, 0f, 0f), new Vector3(0f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[4], new Vector3(0.5f, 0f, 0f), new Vector3(0.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[5], new Vector3(1f, 0f, 0f), new Vector3(1f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[6], new Vector3(1.5f, 0f, 0f), new Vector3(1.5f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            case 8:
                containers[0].SetActive(true);
                level.PopulateContainer(0, containerContents[0]);
                coroutine = MoveContainer(containers[0], new Vector3(-1.75f, yHeightLow, 0f), new Vector3(-1.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[1].SetActive(true);
                level.PopulateContainer(1, containerContents[1]);
                coroutine = MoveContainer(containers[1], new Vector3(-1.25f, yHeightLow, 0f), new Vector3(-1.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[2].SetActive(true);
                level.PopulateContainer(2, containerContents[2]);
                coroutine = MoveContainer(containers[2], new Vector3(-0.75f, yHeightLow, 0f), new Vector3(-0.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[3].SetActive(true);
                level.PopulateContainer(3, containerContents[3]);
                coroutine = MoveContainer(containers[3], new Vector3(-0.25f, yHeightLow, 0f), new Vector3(-0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[4].SetActive(true);
                level.PopulateContainer(4, containerContents[4]);
                coroutine = MoveContainer(containers[4], new Vector3(0.25f, yHeightLow, 0f), new Vector3(0.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[5].SetActive(true);
                level.PopulateContainer(5, containerContents[5]);
                coroutine = MoveContainer(containers[5], new Vector3(0.75f, yHeightLow, 0f), new Vector3(0.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[6].SetActive(true);
                level.PopulateContainer(6, containerContents[6]);
                coroutine = MoveContainer(containers[6], new Vector3(1.25f, yHeightLow, 0f), new Vector3(1.25f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containers[7].SetActive(true);
                level.PopulateContainer(7, containerContents[7]);
                coroutine = MoveContainer(containers[7], new Vector3(1.75f, yHeightLow, 0f), new Vector3(1.75f, 0f, 0f), moveTime);
                StartCoroutine(coroutine);
                containerOnLeftSideOfTray = 0;
                lastContainerToCheck = 6;
                break;

            case -8:
                coroutine = MoveContainer(containers[0], new Vector3(-1.75f, 0f, 0f), new Vector3(-1.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[1], new Vector3(-1.25f, 0f, 0f), new Vector3(-1.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[2], new Vector3(-0.75f, 0f, 0f), new Vector3(-0.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[3], new Vector3(-0.25f, 0f, 0f), new Vector3(-0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[4], new Vector3(0.25f, 0f, 0f), new Vector3(0.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[5], new Vector3(0.75f, 0f, 0f), new Vector3(0.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[6], new Vector3(1.25f, 0f, 0f), new Vector3(1.25f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                coroutine = MoveContainer(containers[7], new Vector3(1.75f, 0f, 0f), new Vector3(1.75f, yHeightLow, 0f), moveTime);
                StartCoroutine(coroutine);
                Invoke("CleanupContainers", moveTime);
                break;

            default:
                break;
        }
    }

    private IEnumerator SwapContainer()
    {
        float elapsedTime = 0, moveTime = level.resetTimer;
        GameObject tempGO;
        int tempInt;
        //Debug.Log(containerOnLeftSideOfTray);
        Vector3 leftPosition = containers[containerOnLeftSideOfTray].transform.position;
        Vector3 rightPosition = containers[containerOnLeftSideOfTray + 1].transform.position;
        //trying to control the normal of the arc that right container slerps across
        rotationCenter.position = (rightPosition + leftPosition) * 0.5f;
        Quaternion startRotation = Quaternion.identity;
        Quaternion endRotation = Quaternion.Euler(0f, -179.9f, 0f);
        containers[containerOnLeftSideOfTray + 1].transform.parent = rotationCenter;

        while (elapsedTime < moveTime)
        {
            containers[containerOnLeftSideOfTray].transform.position = Vector3.Lerp(leftPosition, rightPosition, (elapsedTime / moveTime));
            rotationCenter.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / moveTime));
            //containers[containerOnLeftSideOfTray + 1].transform.position = Vector3.Slerp(rightRelCenter, leftRelCenter, (elapsedTime / moveTime));
            //containers[containerOnLeftSideOfTray + 1].transform.position += centerPosition;
            //what the hell is this line?
            //containers[containerOnLeftSideOfTray + 1].transform.localPosition = new Vector3(containers[containerOnLeftSideOfTray + 1].transform.localPosition.x, containers[containerOnLeftSideOfTray + 1].transform.localPosition.y, -containers[containerOnLeftSideOfTray + 1].transform.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        containers[containerOnLeftSideOfTray].transform.position = rightPosition;
        containers[containerOnLeftSideOfTray + 1].transform.position = leftPosition;
        containers[containerOnLeftSideOfTray + 1].transform.parent = this.transform;
        tempGO = containers[containerOnLeftSideOfTray];
        containers[containerOnLeftSideOfTray] = containers[containerOnLeftSideOfTray + 1];
        containers[containerOnLeftSideOfTray + 1] = tempGO;
        tempInt = containerContents[containerOnLeftSideOfTray];
        containerContents[containerOnLeftSideOfTray] = containerContents[containerOnLeftSideOfTray + 1];
        containerContents[containerOnLeftSideOfTray + 1] = tempInt;
        rotationCenter.rotation = Quaternion.identity;
        yield return null;
    }

    private IEnumerator MoveContainer(GameObject container, Vector3 startPosition, Vector3 endPosition, float moveTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            container.transform.localPosition = Vector3.Lerp(startPosition, endPosition, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public override void SetListenerState(BubbleSortState currentState)
    {
        switch (currentState)
        {
            case BubbleSortState.IntroductionToNextButton:
                ResetContainers(0);
                Debug.Log(gameObject.name + " set to " + currentState.ToString());
                break;

            case BubbleSortState.IntroductionToSwapButton:
                containerContents[0] = (int)Random.Range(5, 9);
                containerContents[1] = (int)Random.Range(1, 4);
                ResetContainers(2);
                break;

            case BubbleSortState.IntroductionToFinishingBubbleSort:
                ResetContainers(-2);
                break;

            case BubbleSortState.IntroductionToThreeElementList01:
                containerContents[0] = (int)Random.Range(4, 6);
                containerContents[1] = (int)Random.Range(7, 9);
                containerContents[2] = (int)Random.Range(1, 3);
                ResetContainers(3);
                break;

            case BubbleSortState.IntroductionToThreeElementList09:
                ResetContainers(-3);
                break;

            case BubbleSortState.BeginnerBubbleSortTask01:
                containerContents[0] = (int)Random.Range(7, 9);
                containerContents[1] = (int)Random.Range(4, 6);
                containerContents[2] = (int)Random.Range(1, 3);
                ResetContainers(3);
                trackNextButton = true;
                atEndOfList = false;
                swapButtonWasPressedThisCycle = false;
                break;

            case BubbleSortState.BeginnerBubbleSortTask01Complete:
                ResetContainers(-3);
                break;

            case BubbleSortState.BeginnerBubbleSortTask02:
                containerContents[0] = (int)Random.Range(7, 9);
                containerContents[1] = (int)Random.Range(10, 12);
                containerContents[2] = (int)Random.Range(1, 3);
                containerContents[3] = (int)Random.Range(4, 6);
                ResetContainers(4);
                trackNextButton = true;
                atEndOfList = false;
                swapButtonWasPressedThisCycle = false;
                break;

            case BubbleSortState.BeginnerBubbleSortTask02Complete:
                ResetContainers(-4);
                break;


            case BubbleSortState.BeginnerBubbleSortTask03:
                containerContents[0] = (int)Random.Range(4, 6);
                containerContents[1] = (int)Random.Range(10, 12);
                containerContents[2] = (int)Random.Range(1, 3);
                containerContents[3] = (int)Random.Range(13, 15);
                containerContents[4] = (int)Random.Range(7, 9);
                ResetContainers(5);
                trackNextButton = true;
                atEndOfList = false;
                swapButtonWasPressedThisCycle = false;
                break;

            case BubbleSortState.BeginnerBubbleSortTask03Complete:
                ResetContainers(-5);
                break;

            case BubbleSortState.BeginnerBubbleSortTask04:
                containerContents[0] = (int)Random.Range(4, 6);
                containerContents[1] = (int)Random.Range(7, 9);
                containerContents[2] = (int)Random.Range(10, 12);
                containerContents[3] = (int)Random.Range(1, 3);
                containerContents[4] = (int)Random.Range(16, 18);
                containerContents[5] = (int)Random.Range(13, 15);
                ResetContainers(6);
                trackNextButton = true;
                atEndOfList = false;
                swapButtonWasPressedThisCycle = false;
                break;

            case BubbleSortState.BeginnerBubbleSortTask04Complete:
                ResetContainers(-6);
                break;

            case BubbleSortState.BeginnerBubbleSortTask05:
                containerContents[0] = (int)Random.Range(16, 18);
                containerContents[1] = (int)Random.Range(7, 9);
                containerContents[2] = (int)Random.Range(4, 6);
                containerContents[3] = (int)Random.Range(19, 21);
                containerContents[4] = (int)Random.Range(1, 3);
                containerContents[5] = (int)Random.Range(10, 12);
                containerContents[6] = (int)Random.Range(13, 15);
                ResetContainers(7);
                trackNextButton = true;
                atEndOfList = false;
                swapButtonWasPressedThisCycle = false;
                break;

            case BubbleSortState.BeginnerBubbleSortTask05Complete:
                ResetContainers(-7);
                break;

            case BubbleSortState.BeginnerBubbleSortTask06:
                containerContents[0] = (int)Random.Range(4, 6);
                containerContents[1] = (int)Random.Range(1, 3);
                containerContents[2] = (int)Random.Range(13, 15);
                containerContents[3] = (int)Random.Range(22, 24);
                containerContents[4] = (int)Random.Range(16, 18);
                containerContents[5] = (int)Random.Range(7, 9);
                containerContents[6] = (int)Random.Range(19, 21);
                containerContents[7] = (int)Random.Range(10, 12);
                ResetContainers(8);
                trackNextButton = true;
                atEndOfList = false;
                swapButtonWasPressedThisCycle = false;
                break;

            case BubbleSortState.BeginnerBubbleSortTask06Complete:
                ResetContainers(-8);
                break;

            default:
                break;
        }
    }
}
