using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasUIFunctions : MonoBehaviour
{
    public IntroSceneObjects introSceneObjects;
    
    private int currentSlide = 0;
    private Scene currentScene;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();

        switch(currentScene.name)
        {
            case "IntroScene":
                currentSlide = -1;
                IntroSequenceNext();
                introSceneObjects.introImage.SetActive(true);
                introSceneObjects.backButton.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void IntroSequenceBack()
    {
        currentSlide -= 2;
        IntroSequenceNext();
    }

    public void IntroSequenceNext()
    {
        currentSlide++;
        int textSlideCount = introSceneObjects.textSlides.Length;
        if(currentSlide < 0 || currentSlide >= textSlideCount) return;
        
        if(currentSlide == 0)
        {
            introSceneObjects.textArea.alignment = TextAlignmentOptions.Center;
            introSceneObjects.introImage.SetActive(true);
            introSceneObjects.backButton.SetActive(false);
        }
        else if(currentSlide == 1)
        {
            introSceneObjects.textArea.alignment = TextAlignmentOptions.MidlineLeft;
            introSceneObjects.introImage.SetActive(false);
            introSceneObjects.backButton.SetActive(true);
        }
        else if(currentSlide == textSlideCount - 2)
        {
            introSceneObjects.nextButton.SetActive(true);
        }
        else if(currentSlide == textSlideCount - 1)
        {
            introSceneObjects.nextButton.SetActive(false);
        }

        introSceneObjects.textArea.text = introSceneObjects.textSlides[currentSlide];
    }

    [System.Serializable]
    public struct IntroSceneObjects
    {
        [TextArea]
        public string[] textSlides;
        public TMP_Text textArea;
        public GameObject introImage;
        public GameObject backButton;
        public GameObject nextButton;
    }
}