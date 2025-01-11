using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    private Vector3 originalScale;

    private void Start()
    {
        originalScale = playButton.transform.localScale;

        AddHoverEffect(playButton);
        AddHoverEffect(quitButton);
    }

    private void AddHoverEffect(Button button)
    {
        
        EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => OnHoverEnter(button));

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => OnHoverExit(button));

        eventTrigger.triggers.Add(pointerEnter);
        eventTrigger.triggers.Add(pointerExit);
    }

    private void OnHoverEnter(Button button)
    {
        button.transform.localScale = originalScale * 1.1f; 
    }

    private void OnHoverExit(Button button)
    {
        button.transform.localScale = originalScale; 
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        
        Application.Quit();
    }
}
