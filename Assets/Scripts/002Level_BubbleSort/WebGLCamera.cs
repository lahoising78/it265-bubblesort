using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLCamera : Listener {

    private Camera mainCamera;
    private IEnumerator coroutine;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
    }

    private IEnumerator CameraTransition(Camera mainCam, float start, float end, float moveTime = 2f, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            mainCam.fieldOfView = Mathf.Lerp(start, end, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCam.fieldOfView = end;
        yield return null;
    }

    public override void SetListenerState(BubbleSortState currentState)
    {
        switch(currentState)
        {
            case BubbleSortState.BeginnerBubbleSortTask01: //3 containers = 26 FOV
                mainCamera.fieldOfView = 26f;
                break;

            case BubbleSortState.BeginnerBubbleSortTask02: //4 containers = 26 FOV
                mainCamera.fieldOfView = 26f;
                break;

            case BubbleSortState.BeginnerBubbleSortTask03: //5 containers = 30 FOV
                coroutine = CameraTransition(mainCamera, mainCamera.fieldOfView, 30f);
                StartCoroutine(coroutine);
                break;

            case BubbleSortState.BeginnerBubbleSortTask04: //6 containers = 35 FOV
                coroutine = CameraTransition(mainCamera, mainCamera.fieldOfView, 35f);
                StartCoroutine(coroutine);
                break;

            case BubbleSortState.BeginnerBubbleSortTask05: //7 containers = 40 FOV
                coroutine = CameraTransition(mainCamera, mainCamera.fieldOfView, 40f);
                StartCoroutine(coroutine);
                break;

            case BubbleSortState.BeginnerBubbleSortTask06: //8 containers = 45 FOV
                coroutine = CameraTransition(mainCamera, mainCamera.fieldOfView, 45f);
                StartCoroutine(coroutine);
                break;
        }
    }
}
